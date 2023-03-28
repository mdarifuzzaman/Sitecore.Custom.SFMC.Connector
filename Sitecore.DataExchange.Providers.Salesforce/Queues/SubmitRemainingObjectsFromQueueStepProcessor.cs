// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.SubmitRemainingObjectsFromQueueStepProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common.Models.Xml;
using Salesforce.Force;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  public class SubmitRemainingObjectsFromQueueStepProcessor : BaseSubmitQueuePipelineProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ObjectQueuePlugin entityQueuePlugin = this.GetEntityQueuePlugin(pipelineContext);
      QueueOperationSettings operationSettingsPlugin = this.GetQueueOperationSettingsPlugin(pipelineStep);
      IDictionary<string, ConcurrentDictionary<int, SalesforceObjectQueue>> queues = entityQueuePlugin.GetQueues();
      ConcurrentDictionary<int, SalesforceObjectQueue> concurrentDictionary;
      if (!queues.TryGetValue(operationSettingsPlugin.ObjectName, out concurrentDictionary))
        return;
      List<SObjectList<SObject>> inputList1 = new List<SObjectList<SObject>>();
      List<SObjectList<SObject>> inputList2 = new List<SObjectList<SObject>>();
      foreach (KeyValuePair<int, SalesforceObjectQueue> keyValuePair in concurrentDictionary)
      {
        if (keyValuePair.Value.InsertList.Any<SObject>())
          inputList1.Add(keyValuePair.Value.InsertList);
        if (keyValuePair.Value.UpdateList.Any<SObject>())
          inputList2.Add(keyValuePair.Value.UpdateList);
      }
      this.ExecuteTelemetry(pipelineStep, pipelineContext, logger);
      this.SubmitBatch(pipelineStep, pipelineContext, BulkConstants.OperationType.Update, inputList2, logger);
      IEnumerable<ResultObject> data = this.SubmitBatch(pipelineStep, pipelineContext, BulkConstants.OperationType.Insert, inputList1, logger);
      queues[operationSettingsPlugin.ObjectName] = new ConcurrentDictionary<int, SalesforceObjectQueue>();
      IterableDataSettings newPlugin = new IterableDataSettings((IEnumerable) data);
      pipelineContext.AddPlugin<IterableDataSettings>(newPlugin);
    }

    private void ExecuteTelemetry(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      try
      {
        if (pipelineStep == null || pipelineContext.PipelineBatchContext == null || pipelineContext.PipelineBatchContext.CurrentPipelineBatch == null || string.IsNullOrEmpty(pipelineContext.PipelineBatchContext.CurrentPipelineBatch.Name) || string.IsNullOrEmpty(pipelineStep.Name) || !(pipelineContext.PipelineBatchContext.CurrentPipelineBatch.Name == "xConnect Contacts to Salesforce Sync") || !(pipelineContext.CurrentPipeline.Name == "Read Contacts from xConnect Pipeline") || !(pipelineStep.Name == "Submit Remaining Contacts In Salesforce Object Queue"))
          return;
        Sitecore.DataExchange.Providers.Salesforce.Telemetry.TrackConnectorsSfCrmXConnectContactsToSfSyncExecute();
      }
      catch (Exception ex)
      {
        logger.Error("ExecuteTelemetry " + ex.InnerException.Message);
      }
    }
  }
}
