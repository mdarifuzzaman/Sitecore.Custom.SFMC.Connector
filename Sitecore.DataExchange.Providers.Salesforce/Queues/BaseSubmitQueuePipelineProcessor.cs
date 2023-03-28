// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.BaseSubmitQueuePipelineProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common;
using Salesforce.Common.Models.Xml;
using Salesforce.Force;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Salesforce.Endpoints;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  public abstract class BaseSubmitQueuePipelineProcessor : BasePipelineStepProcessor
  {
    public virtual IEnumerable<ResultObject> SubmitBatch(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      BulkConstants.OperationType operationType,
      List<SObjectList<SObject>> inputList,
      ILogger logger)
    {
      List<ResultObject> resultObjectList = new List<ResultObject>();
      ObjectQueuePlugin entityQueuePlugin = this.GetEntityQueuePlugin(pipelineContext);
      if (entityQueuePlugin == null)
      {
        logger.Error("No Object Queue Plugin Is set.");
        return (IEnumerable<ResultObject>) resultObjectList;
      }
      Endpoint endpoint = entityQueuePlugin.Endpoint;
      AuthenticationClient authenticationClient = endpoint != null ? this.GetAuthenticationClient(endpoint, logger) : throw new ArgumentNullException("endpoint");
      if (authenticationClient == null)
      {
        logger.Error("No authentication client is set on the endpoint.", (object) ("endpoint: " + endpoint.Name));
        pipelineContext.CriticalError = true;
        return (IEnumerable<ResultObject>) resultObjectList;
      }
      if (inputList.Any<SObjectList<SObject>>())
      {
        ForceClient client = new ForceClient(authenticationClient.InstanceUrl, authenticationClient.AccessToken, authenticationClient.ApiVersion);
        QueueOperationSettings settings = this.GetQueueOperationSettingsPlugin(pipelineStep);
        using (client)
        {
          List<BatchResultList> batchResultListList = new List<BatchResultList>();
          try
          {
            batchResultListList = Task.Run<List<BatchResultList>>((Func<Task<List<BatchResultList>>>) (() => client.RunJobAndPollAsync<SObject>(settings.ObjectName, operationType, (IEnumerable<ISObjectList<SObject>>) inputList))).Result;
          }
          catch (Exception ex)
          {
            logger.Error("Exception raised while submitting Batch to Salesforce");
            logger.Error(ex.Message);
            logger.Debug(ex.StackTrace);
            if (ex.InnerException != null)
            {
              logger.Error(ex.InnerException.Message);
              logger.Debug(ex.InnerException.StackTrace);
            }
          }
          for (int index1 = 0; index1 < batchResultListList.Count; ++index1)
          {
            List<BatchResult> items = batchResultListList[index1].Items;
            if (inputList.Count >= index1 + 1)
            {
              SObjectList<SObject> input = inputList[index1];
              for (int index2 = 0; index2 < items.Count && input.Count >= index2 + 1; ++index2)
              {
                BatchResult batchResult = items[index2];
                if (!batchResult.Success)
                {
                  logger.Error("Record was not saved:" + batchResult.Errors.StatusCode);
                  logger.Error("Record was not saved:" + batchResult.Errors.Message);
                }
                else if (input[index2].ContainsKey(settings.SitecoreIdFieldName) && batchResult.Id != null)
                  resultObjectList.Add(new ResultObject()
                  {
                    SourceId = input[index2][settings.SitecoreIdFieldName].ToString(),
                    TargetId = batchResult.Id
                  });
              }
            }
            else
              break;
          }
        }
      }
      return (IEnumerable<ResultObject>) resultObjectList;
    }

    protected virtual AuthenticationClient GetAuthenticationClient(
      Endpoint endpoint,
      ILogger logger)
    {
      return endpoint.GetPlugin<AuthenticationClientSettings>().AuthenticationClient;
    }

    protected virtual Endpoint GetEndpoint(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      EndpointSettings endpointSettings = pipelineStep.GetEndpointSettings();
      if (endpointSettings == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The pipeline step is missing a plugin.", new string[1]
        {
          "plugin: " + typeof (EndpointSettings).FullName
        });
        return (Endpoint) null;
      }
      Endpoint endpointFrom = endpointSettings.EndpointFrom;
      if (endpointFrom != null)
        return endpointFrom;
      this.Log(new Action<string>(logger.Error), pipelineContext, "The pipeline step is missing an endpoint to read from.", new string[2]
      {
        "plugin: " + typeof (EndpointSettings).FullName,
        "plugin: EndpointFrom"
      });
      return (Endpoint) null;
    }

    protected virtual ObjectQueuePlugin GetEntityQueuePlugin(
      PipelineContext pipelineContext)
    {
      return pipelineContext.GetPlugin<ObjectQueuePlugin>(true);
    }

    protected virtual QueueOperationSettings GetQueueOperationSettingsPlugin(
      PipelineStep pipelineStep)
    {
      return pipelineStep.GetPlugin<QueueOperationSettings>(true);
    }
  }
}
