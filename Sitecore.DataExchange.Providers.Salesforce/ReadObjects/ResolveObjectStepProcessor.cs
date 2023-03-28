// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ResolveObjectStepProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common;
using Salesforce.Force;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Salesforce.Endpoints;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Dynamic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  public class ResolveObjectStepProcessor : 
    BaseResolveObjectFromRepositoryEndpointStepProcessor<string>
  {
    private static IConverter<ReadObjectsSettings, ObjectReaderContext> _contextConverter = (IConverter<ReadObjectsSettings, ObjectReaderContext>) new ObjectReaderContextConverter();
    private static IObjectReader _reader = (IObjectReader) new ObjectReader();

    public override object FindExistingObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (identifierValue == null)
        throw new ArgumentException("The value cannot be null.", nameof (identifierValue));
      Endpoint endpoint = this.GetEndpoint(pipelineStep, pipelineContext, logger);
      if (endpoint == null)
        throw new ArgumentNullException("endpoint");
      if (pipelineStep == null)
        throw new ArgumentNullException(nameof (pipelineStep));
      if (pipelineContext == null)
        throw new ArgumentNullException(nameof (pipelineContext));
      ObjectReaderContext objectReaderContext = this.GetObjectReaderContext(endpoint, identifierValue);
      if (objectReaderContext == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot read data because no context was created.", new string[1]
        {
          "missing context type: " + typeof (ObjectReaderContext).FullName
        });
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      IObjectReader objectReader = this.GetObjectReader(endpoint, pipelineStep, pipelineContext, logger);
      if (objectReader == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot read data because no entity reader is set on the processor.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      AuthenticationClient authenticationClient = this.GetAuthenticationClient(endpoint, pipelineStep, pipelineContext, logger);
      if (authenticationClient == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "No authentication client is set on the endpoint.", new string[1]
        {
          "endpoint: " + endpoint.Name
        });
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      try
      {
        ForceClient client = new ForceClient(authenticationClient.InstanceUrl, authenticationClient.AccessToken, authenticationClient.ApiVersion);
        ExpandoObject existingObject = objectReader.ReadObject(client, objectReaderContext);
        if (existingObject != null)
        {
          this.Log(new Action<string>(logger.Debug), pipelineContext, "Object was resolved.", new string[1]
          {
            "identifier: " + identifierValue
          });
          this.SetRepositoryStatusSettings(RepositoryObjectStatus.Exists, pipelineContext);
          return (object) existingObject;
        }
      }
      catch (Exception ex)
      {
        this.LogException(ex, new Action<string>(logger.Error), pipelineContext, "Exception thrown when reading objects from Salesforce.", new string[2]
        {
          "endpoint: " + endpoint.Name,
          "object type: " + objectReaderContext.ObjectType
        });
        pipelineContext.CriticalError = true;
      }
      return (object) null;
    }

    public override object CreateNewObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return (object) new ExpandoObject();
    }

    [Obsolete("This method is obsoleted. Use GetObjectReaderContext(Endpoint endpoint, string identifierValue) instead.")]
    protected virtual ObjectReaderContext GetObjectReaderContext(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ReadObjectsSettings plugin = pipelineStep.GetPlugin<ReadObjectsSettings>();
      ConvertResult<ObjectReaderContext> convertResult = this.GetObjectReaderContextConverter(endpoint, pipelineStep, pipelineContext, logger).Convert(plugin);
      return convertResult.WasConverted ? convertResult.ConvertedValue : (ObjectReaderContext) null;
    }

    protected virtual ObjectReaderContext GetObjectReaderContext(
      Endpoint endpoint,
      string identifierValue)
    {
      ReadObjectsSettings plugin = this.PipelineStep.GetPlugin<ReadObjectsSettings>();
      ConvertResult<ObjectReaderContext> convertResult = this.GetObjectReaderContextConverter(endpoint, this.PipelineStep, this.PipelineContext, this.Logger).Convert(plugin);
      if (!convertResult.WasConverted)
        return (ObjectReaderContext) null;
      if (string.IsNullOrWhiteSpace(plugin.IdentifierFieldName))
        return convertResult.ConvertedValue;
      convertResult.ConvertedValue.FilterExpression = plugin.IdentifierFieldName + " = '" + identifierValue + "'";
      return convertResult.ConvertedValue;
    }

    protected virtual IObjectReader GetObjectReader(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return ResolveObjectStepProcessor._reader;
    }

    protected virtual IConverter<ReadObjectsSettings, ObjectReaderContext> GetObjectReaderContextConverter(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return ResolveObjectStepProcessor._contextConverter;
    }

    protected virtual AuthenticationClient GetAuthenticationClient(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return endpoint.GetPlugin<AuthenticationClientSettings>().AuthenticationClient;
    }

    protected override string ConvertValueToIdentifier(
      object identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return identifierValue.ToString();
    }
  }
}
