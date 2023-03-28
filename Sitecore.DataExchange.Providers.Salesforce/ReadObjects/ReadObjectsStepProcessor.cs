// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ReadObjectsStepProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common;
using Salesforce.Force;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Salesforce.Endpoints;
using Sitecore.DataExchange.Providers.Salesforce.Query;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  [RequiredEndpointPlugins(new Type[] {typeof (AuthenticationClientSettings)})]
  [RequiredPipelineStepPlugins(new Type[] {typeof (ReadObjectsSettings)})]
  public class ReadObjectsStepProcessor : BaseReadDataStepProcessor
  {
    private static IConverter<ReadObjectsSettings, ObjectReaderContext> _contextConverter = (IConverter<ReadObjectsSettings, ObjectReaderContext>) new ObjectReaderContextConverter();
    private static IObjectReader _reader = (IObjectReader) new ObjectReader();
    private static IDateRangeConditionHelper _helper = (IDateRangeConditionHelper) new DateRangeConditionHelper();

    protected override void ReadData(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (endpoint == null)
        throw new ArgumentNullException(nameof (endpoint));
      if (pipelineStep == null)
        throw new ArgumentNullException(nameof (pipelineStep));
      if (pipelineContext == null)
        throw new ArgumentNullException(nameof (pipelineContext));
      ObjectReaderContext objectReaderContext = this.GetObjectReaderContext(endpoint, pipelineStep, pipelineContext, logger);
      if (objectReaderContext == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot read data because no context was created.", new string[1]
        {
          "missing context type: " + typeof (ObjectReaderContext).FullName
        });
        pipelineContext.CriticalError = true;
      }
      else
      {
        string filterExpression = this.GetFilterExpression(endpoint, pipelineStep, pipelineContext, logger);
        if (filterExpression != null)
          objectReaderContext.FilterExpression = filterExpression;
        IObjectReader objectReader = this.GetObjectReader(endpoint, pipelineStep, pipelineContext, logger);
        if (objectReader == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot read data because no entity reader is set on the processor.", Array.Empty<string>());
          pipelineContext.CriticalError = true;
        }
        else
        {
          AuthenticationClient authenticationClient = this.GetAuthenticationClient(endpoint, pipelineStep, pipelineContext, logger);
          if (authenticationClient == null)
          {
            this.Log(new Action<string>(logger.Error), pipelineContext, "No authentication client is set on the endpoint.", new string[1]
            {
              "endpoint: " + endpoint.Name
            });
            pipelineContext.CriticalError = true;
          }
          else
          {
            try
            {
              ForceClient client = new ForceClient(authenticationClient.InstanceUrl, authenticationClient.AccessToken, authenticationClient.ApiVersion);
              IEnumerable<ExpandoObject> data = objectReader.ReadObjects(client, objectReaderContext);
              if (data == null)
              {
                this.Log(new Action<string>(logger.Error), pipelineContext, "The object reader returned null when reading objects.", new string[2]
                {
                  "endpoint: " + endpoint.Name,
                  "entity: " + objectReaderContext.ObjectType
                });
                pipelineContext.CriticalError = true;
              }
              else
              {
                IterableDataSettings newPlugin = new IterableDataSettings((IEnumerable) data);
                this.Log(new Action<string>(logger.Debug), pipelineContext, "Objects were read from Salesforce.", new string[2]
                {
                  "endpoint: " + endpoint.Name,
                  "object type: " + objectReaderContext.ObjectType
                });
                this.ExecuteTelemetry(pipelineStep, pipelineContext, logger);
                pipelineContext.AddPlugin<IterableDataSettings>(newPlugin);
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
          }
        }
      }
    }

    protected virtual AuthenticationClient GetAuthenticationClient(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return endpoint.GetPlugin<AuthenticationClientSettings>().AuthenticationClient;
    }

    protected virtual IObjectReader GetObjectReader(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return ReadObjectsStepProcessor._reader;
    }

    protected virtual IDateRangeConditionHelper GetDateRangeConditionHelper(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return ReadObjectsStepProcessor._helper;
    }

    protected virtual IConverter<ReadObjectsSettings, ObjectReaderContext> GetObjectReaderContextConverter(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return ReadObjectsStepProcessor._contextConverter;
    }

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

    protected virtual object GetContextObject(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Guid contextObjectLocation = pipelineStep.GetPlugin<ReadObjectsSettings>().ContextObjectLocation;
      return contextObjectLocation == Guid.Empty ? (object) null : this.GetObjectFromPipelineContext(contextObjectLocation, pipelineContext, logger);
    }

    protected virtual string GetFilterExpression(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      object contextObject = this.GetContextObject(pipelineStep, pipelineContext, logger);
      ReadObjectsSettings plugin1 = pipelineStep.GetPlugin<ReadObjectsSettings>();
      DateRangeSettings plugin2 = pipelineContext.GetPlugin<DateRangeSettings>();
      List<string> stringList = new List<string>();
      if (plugin1.FilterExpression != null)
        stringList.Add(plugin1.FilterExpression.ToExpression(contextObject));
      if (pipelineContext != null && !plugin1.ExcludeUseDeltaSettings && plugin2 != null)
      {
        string lowerBoundCondition = this.GetLowerBoundCondition(plugin1, plugin2);
        if (lowerBoundCondition != null)
        {
          stringList.Add(lowerBoundCondition);
          this.Log(new Action<string>(logger.Info), pipelineContext, string.Format("Condition added to read objects modified on or after {0}.", (object) plugin2.LowerBound), new string[1]
          {
            "object: " + plugin1.ObjectName
          });
        }
        string upperBoundCondition = this.GetUpperBoundCondition(plugin1, plugin2);
        if (upperBoundCondition != null)
        {
          stringList.Add(upperBoundCondition);
          this.Log(new Action<string>(logger.Info), pipelineContext, string.Format("Condition added to read objects modified on or before {0}.", (object) plugin2.UpperBound), new string[1]
          {
            "object: " + plugin1.ObjectName
          });
        }
      }
      if (stringList.Count == 0)
        return (string) null;
      return stringList.Count == 1 ? stringList[0] : "(" + string.Join(" " + plugin1.FilterExpression.LogicalOperator + " ", stringList.ToArray()) + ")";
    }

    protected virtual string GetLowerBoundCondition(
      ReadObjectsSettings readObjectsSettings,
      DateRangeSettings dateRangeSettings)
    {
      return dateRangeSettings.LowerBound <= SalesforceDateTime.MinValue ? (string) null : "LastModifiedDate" + " >= " + this.ConvertDateTimeForSalesforce(dateRangeSettings.LowerBound);
    }

    protected virtual string GetUpperBoundCondition(
      ReadObjectsSettings readObjectsSettings,
      DateRangeSettings dateRangeSettings)
    {
      return dateRangeSettings.UpperBound >= SalesforceDateTime.MaxValue ? (string) null : "LastModifiedDate" + " <= " + this.ConvertDateTimeForSalesforce(dateRangeSettings.UpperBound);
    }

    protected virtual string ConvertDateTimeForSalesforce(DateTime dateTime)
    {
      dateTime = dateTime.ToUniversalTime();
      return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }

    private void ExecuteTelemetry(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      try
      {
        if (pipelineStep == null || pipelineContext.PipelineBatchContext == null || pipelineContext.PipelineBatchContext.CurrentPipelineBatch == null || string.IsNullOrEmpty(pipelineContext.PipelineBatchContext.CurrentPipelineBatch.Name) || string.IsNullOrEmpty(pipelineStep.Name))
          return;
        if (pipelineContext.PipelineBatchContext.CurrentPipelineBatch.Name == "Salesforce Campaigns to xConnect Sync" && pipelineContext.CurrentPipeline.Name == "Read Campaigns from Salesforce" && pipelineStep.Name == "Read Salesforce Campaigns")
          Sitecore.DataExchange.Providers.Salesforce.Telemetry.TrackConnectorsSfCrmSfCampaignsToXConnectSyncExecute();
        else if (pipelineContext.PipelineBatchContext.CurrentPipelineBatch.Name == "Salesforce Contacts to xConnect Sync" && pipelineContext.CurrentPipeline.Name == "Read Contacts from Salesforce Pipeline" && pipelineStep.Name == "Read Contacts from Salesforce")
        {
          Sitecore.DataExchange.Providers.Salesforce.Telemetry.TrackConnectorsSfCrmSfContactsToXConnectSyncExecute();
        }
        else
        {
          if (!(pipelineContext.PipelineBatchContext.CurrentPipelineBatch.Name == "Salesforce Tasks to xConnect Sync") || !(pipelineContext.CurrentPipeline.Name == "Read Tasks from Salesforce Pipeline") || !(pipelineStep.Name == "Read Tasks from Salesforce"))
            return;
          Sitecore.DataExchange.Providers.Salesforce.Telemetry.TrackConnectorsSfCrmSfTasksToXConnectSyncExecute();
        }
      }
      catch (Exception ex)
      {
        logger.Error("ExecuteTelemetry " + ex.InnerException.Message);
      }
    }
  }
}
