// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.AddObjectToQueueStepProcesor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common.Models.Xml;
using Salesforce.Force;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.Salesforce.Converters;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (BatchEntryStorageSettings)})]
  internal class AddObjectToQueueStepProcesor : BaseSubmitQueuePipelineProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ObjectQueuePlugin entityQueuePlugin = this.GetEntityQueuePlugin(pipelineContext);
      QueueOperationSettings operationSettingsPlugin = this.GetQueueOperationSettingsPlugin(pipelineStep);
      if (entityQueuePlugin == null)
        return;
      SalesforceObjectQueue queue = entityQueuePlugin.GetQueue(operationSettingsPlugin.ObjectName, pipelineContext.GetCurrentThreadId());
      SObject sobject = this.NormalizeEntity(this.ResolveObject(pipelineStep, pipelineContext, logger));
      if (sobject != null && sobject.Count > 0)
      {
        object obj;
        if ((sobject.ContainsKey("Id") || sobject.ContainsKey("id")) && (sobject.TryGetValue("Id", out obj) || sobject.TryGetValue("id", out obj)) && !string.IsNullOrWhiteSpace(obj.ToString()))
        {
          queue.UpdateList.Add(sobject);
          if (queue.UpdateList.Count < entityQueuePlugin.MinimumBatchSize)
            return;
          PipelineStep pipelineStep1 = pipelineStep;
          PipelineContext pipelineContext1 = pipelineContext;
          BulkConstants.OperationType update = BulkConstants.OperationType.Update;
          List<SObjectList<SObject>> inputList = new List<SObjectList<SObject>>();
          inputList.Add(queue.UpdateList);
          ILogger logger1 = logger;
          this.SubmitBatch(pipelineStep1, pipelineContext1, update, inputList, logger1);
          queue.UpdateList = new SObjectList<SObject>();
        }
        else
        {
          queue.InsertList.Add(sobject);
          if (queue.InsertList.Count < entityQueuePlugin.MinimumBatchSize)
            return;
          PipelineStep pipelineStep2 = pipelineStep;
          PipelineContext pipelineContext2 = pipelineContext;
          BulkConstants.OperationType insert = BulkConstants.OperationType.Insert;
          List<SObjectList<SObject>> inputList = new List<SObjectList<SObject>>();
          inputList.Add(queue.InsertList);
          ILogger logger2 = logger;
          IterableDataSettings newPlugin = new IterableDataSettings((IEnumerable) this.SubmitBatch(pipelineStep2, pipelineContext2, insert, inputList, logger2));
          pipelineContext.AddPlugin<IterableDataSettings>(newPlugin);
          queue.InsertList = new SObjectList<SObject>();
        }
      }
      else
        logger.Debug("New entry for Queue is empty, skipping.");
    }

    protected virtual SObject NormalizeEntity(SObject entity)
    {
      SObject sobject = new SObject();
      foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) entity)
      {
        if (keyValuePair.Value is string)
          sobject[keyValuePair.Key] = (object) WebUtility.HtmlEncode((string) keyValuePair.Value);
        else
          sobject[keyValuePair.Key] = keyValuePair.Value;
      }
      return sobject;
    }

    protected virtual SObject ResolveObject(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      BatchEntryStorageSettings plugin = pipelineStep.GetPlugin<BatchEntryStorageSettings>();
      return plugin == null ? (SObject) null : this.ConvertResult(this.GetObjectFromPipelineContext(plugin.LocationForNewEntry, pipelineContext, logger));
    }

    protected virtual SObject ConvertResult(object obj) => ExpandoSObjectConverter.Convert(obj as ExpandoObject);
  }
}
