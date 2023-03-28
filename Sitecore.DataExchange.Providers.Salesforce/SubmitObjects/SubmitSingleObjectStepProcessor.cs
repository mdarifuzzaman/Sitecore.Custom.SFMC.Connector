// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.SubmitObjects.SubmitSingleObjectStepProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common;
using Salesforce.Common.Models.Json;
using Salesforce.Common.Models.Xml;
using Salesforce.Force;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Salesforce.Converters;
using Sitecore.DataExchange.Providers.Salesforce.Endpoints;
using Sitecore.DataExchange.Providers.Salesforce.Plugins;
using Sitecore.DataExchange.Providers.Salesforce.Queues;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Sitecore.DataExchange.Providers.Salesforce.SubmitObjects
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (ObjectLocationSettings), typeof (SubmitObjectSettings)})]
  [RequiredPipelineContextPlugins(new Type[] {typeof (RepositoryObjectStatusSettings)})]
  [RequiredEndpointPlugins(new Type[] {typeof (AuthenticationClientSettings)})]
  public class SubmitSingleObjectStepProcessor : BasePipelineStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Endpoint endpointTo = this.GetEndpointTo();
      AuthenticationClient authenticationClient = this.GetAuthenticationClient();
      if (authenticationClient == null)
      {
        logger.Error("No authentication client is set on the endpoint.", (object) ("endpoint: " + endpointTo.Name));
        pipelineContext.CriticalError = true;
      }
      else
        this.SubmitObject(new ForceClient(authenticationClient.InstanceUrl, authenticationClient.AccessToken, authenticationClient.ApiVersion));
    }

    protected virtual void SubmitObject(ForceClient client)
    {
      SObject sobject = this.ResolveSObject();
      string objectName = this.GetSubmitObjectSettings().ObjectName;
      if (sobject == null)
      {
        this.Logger.Warn("Salesforce object cannot be submitted. Object is null. (object name: " + objectName + ")");
      }
      else
      {
        RepositoryObjectStatusSettings repositoryObjectSettings = this.GetRepositoryObjectSettings();
        if (sobject.ContainsKey("Id") && repositoryObjectSettings.Status == RepositoryObjectStatus.Exists)
          this.UpdateObject(client, sobject, objectName);
        else
          this.CreateObject(client, sobject, objectName);
      }
    }

    protected virtual void CreateObject(ForceClient client, SObject sobject, string objectName)
    {
      RepositoryObjectStatusSettings repositoryObjectSettings = this.GetRepositoryObjectSettings();
      this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Trying to update salesforce object.", Array.Empty<string>());
      try
      {
        SuccessResponse result = client.CreateAsync(objectName, (object) sobject).Result;
        if (result.Success)
        {
          repositoryObjectSettings.Status = RepositoryObjectStatus.Exists;
          this.Log(new Action<string>(this.Logger.Info), this.PipelineContext, "Salesforce object was successfuly created. (object name: " + objectName + ")", Array.Empty<string>());
          this.SetResultObjectToResultLocation(this.CreateResultObject(sobject, result.Id));
        }
        else
          this.Log(new Action<string>(this.Logger.Error), this.PipelineContext, "Error during creating. (object name: " + objectName + ")", Array.Empty<string>());
      }
      catch (Exception ex)
      {
        this.LogException(ex, new Action<string>(this.Logger.Error), this.PipelineContext, "Error duting creating. (object name: " + objectName + ")", Array.Empty<string>());
        this.PipelineContext.CriticalError = true;
      }
    }

    protected virtual ResultObject CreateResultObject(SObject sobject, string sobjectId)
    {
      SubmitObjectSettings submitObjectSettings = this.GetSubmitObjectSettings();
      return new ResultObject()
      {
        SourceId = string.IsNullOrWhiteSpace(submitObjectSettings.SitecoreIdFieldName) || !sobject.ContainsKey(submitObjectSettings.SitecoreIdFieldName) ? (string) null : sobject[submitObjectSettings.SitecoreIdFieldName].ToString(),
        TargetId = sobjectId
      };
    }

    protected virtual void SetResultObjectToResultLocation(ResultObject resultObject)
    {
      Guid objectLocationId = this.GetObjectLocationSettings().ResultObjectLocationId;
      if (objectLocationId == Guid.Empty)
        this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Result object location is not set.", Array.Empty<string>());
      else if (resultObject == null)
        this.Log(new Action<string>(this.Logger.Warn), this.PipelineContext, string.Format("Cannot set {0} to a result object location. Result object is null. (result object location id: {1})", (object) "ResultObject", (object) objectLocationId), Array.Empty<string>());
      else
        this.SetObjectOnPipelineContext((object) resultObject, objectLocationId, this.PipelineContext, this.Logger);
    }

    protected virtual void UpdateObject(ForceClient client, SObject sobject, string objectName)
    {
      string str = sobject["Id"].ToString();
      this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Trying to update salesforce object. (object id: " + str, Array.Empty<string>());
      SObject record = new SObject();
      foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) sobject)
      {
        if (keyValuePair.Key != "Id")
          record[keyValuePair.Key] = keyValuePair.Value;
      }
      try
      {
        if (client.UpdateAsync(objectName, str, (object) record).Result.Success)
        {
          this.Log(new Action<string>(this.Logger.Info), this.PipelineContext, "Salesforce object was successfuly updated. (object name: " + objectName + ")", Array.Empty<string>());
          this.SetResultObjectToResultLocation(this.CreateResultObject(sobject, str));
        }
        else
        {
          this.Logger.Error("Error duting updating. (object name: " + objectName + ")");
          this.Log(new Action<string>(this.Logger.Error), this.PipelineContext, "Error during updating. (object name: " + objectName + ")", Array.Empty<string>());
        }
      }
      catch (Exception ex)
      {
        this.LogException(ex, new Action<string>(this.Logger.Error), this.PipelineContext, "Error duting updating. (object name: " + objectName + ")", Array.Empty<string>());
        this.PipelineContext.CriticalError = true;
      }
    }

    protected virtual SObject ResolveSObject()
    {
      Guid objectLocationId = this.GetObjectLocationSettings().ObjectLocationId;
      return objectLocationId == Guid.Empty ? (SObject) null : ExpandoSObjectConverter.Convert(this.GetObjectFromPipelineContext(objectLocationId, this.PipelineContext, this.Logger) as ExpandoObject);
    }

    protected virtual Endpoint GetEndpointTo() => this.PipelineStep?.GetPlugin<EndpointSettings>()?.EndpointTo;

    protected virtual AuthenticationClient GetAuthenticationClient() => this.PipelineStep?.GetPlugin<EndpointSettings>()?.EndpointTo?.GetPlugin<AuthenticationClientSettings>()?.AuthenticationClient;

    protected virtual ObjectLocationSettings GetObjectLocationSettings() => this.PipelineStep?.GetPlugin<ObjectLocationSettings>();

    protected virtual RepositoryObjectStatusSettings GetRepositoryObjectSettings() => this.PipelineContext?.GetPlugin<RepositoryObjectStatusSettings>();

    protected virtual SubmitObjectSettings GetSubmitObjectSettings() => this.PipelineStep?.GetPlugin<SubmitObjectSettings>();
  }
}
