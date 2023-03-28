// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.BaseSubmitXConnectBatchStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Retryers;
using Sitecore.DataExchange.VerificationLog;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (RetrySettings)})]
  public abstract class BaseSubmitXConnectBatchStepProcessor : BasePipelineStepProcessor
  {
    public virtual List<IVerificationLogEntry> GetVerificationLogCollection(
      PipelineContext context)
    {
      XConnectVerificationLogCollections newPlugin = context.PipelineBatchContext.GetPlugin<XConnectVerificationLogCollections>();
      if (newPlugin == null)
      {
        newPlugin = new XConnectVerificationLogCollections();
        context.PipelineBatchContext.AddPlugin<XConnectVerificationLogCollections>(newPlugin);
      }
      return newPlugin.GetCollectionForCurrentThread(context);
    }

    protected virtual bool ShouldSubmitBatch(
      IXdbContext client,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return client != null && client.DirectOperations.Any<IXdbOperation>();
    }

    protected virtual string[] GetLogDetails(
      string batchDescription,
      IXdbContext client,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      List<string> stringList = new List<string>();
      if (!string.IsNullOrWhiteSpace(batchDescription))
        stringList.Add("description: " + batchDescription);
      stringList.Add(string.Format("operation count: {0}", (object) client.DirectOperations.Count));
      return stringList.ToArray();
    }

    protected virtual void SubmitBatch(
      string batchDescription,
      IXdbContext client,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (client == null)
      {
        this.SubmitVerificationLogEntries(pipelineContext);
      }
      else
      {
        string[] logDetails = this.GetLogDetails(batchDescription, client, pipelineStep, pipelineContext, logger);
                if (!this.ShouldSubmitBatch(client, pipelineStep, pipelineContext, logger))
                {
                    this.Log(new Action<string>(logger.Info), pipelineContext, "Batch will not be submitted to xConnect.", logDetails);
                    this.SubmitVerificationLogEntries(pipelineContext);
                }
                else
                {
                    var exceptionHandlingSettings = pipelineStep.GetPlugin<ExceptionHandlingSettings>();
                    try
                    {
                        this.Log(new Action<string>(logger.Info), pipelineContext, "Submitting batch to xConnect.", logDetails);
                        IRetryerOnException retryer = this.GetRetryer(pipelineStep, pipelineContext, logger);
                        if (retryer == null)
                            client.SubmitAsync().Wait();
                        else
                            retryer.Retry((Action)(() => client.SubmitAsync().Wait()));
                    }
                    catch (Exception ex)
                    {
                        var aggregateException = ex.InnerException as AggregateException;
                        var messageContainsTheException = aggregateException?.InnerExceptions.All(exception => exception.Message.Contains("Contact does not exists"));
                        if (messageContainsTheException ?? false)
                        {
                            Log(logger.Warn, pipelineContext, "Some contacts have been deleted while processing them, they might not appear in the target system.", logDetails);
                            pipelineContext.Finished = true;
                        }
                        else
                        {
                            if ((exceptionHandlingSettings != null) && exceptionHandlingSettings.ProceedOnException)
                            {
                                LogException(ex, logger.Error, pipelineContext, "Exception while submitting batch to xConnect.", logDetails);
                                pipelineContext.Finished = true;
                            }
                            else
                            {
                                pipelineContext.CriticalError = true;
                                throw new XConnectClientException("Exception while submitting batch to xConnect.", ex);
                            }
                        }
                    }
                    this.OnBatchProcessed(client.LastBatchId, (ICollection<IXdbOperation>)client.LastBatch, pipelineContext, client);
                }
      }
    }

    protected virtual IRetryerOnException GetRetryer(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IRetryerOnException retryer = pipelineStep != null ? pipelineStep.GetRetrySettings()?.RetryerOnException : (IRetryerOnException) null;
      if (retryer == null)
        return (IRetryerOnException) null;
      retryer.Logger = logger;
      return retryer;
    }

    protected virtual void SubmitBatch(
      IXdbContext client,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      this.SubmitBatch((string) null, client, pipelineStep, pipelineContext, logger);
    }

    protected virtual IXdbContext GetClient(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientPlugin plugin = pipelineContext.GetPlugin<XConnectClientPlugin>(true);
      if (plugin == null)
        return (IXdbContext) null;
      int? threadId = this.GetThreadId(pipelineContext);
      return plugin.GetClient(!threadId.HasValue ? 0 : threadId.Value);
    }

    protected virtual void OnBatchProcessed(
      Guid batchId,
      ICollection<IXdbOperation> operations,
      PipelineContext pipelineContext,
      IXdbContext client = null)
    {
      PipelineBatch currentPipelineBatch = pipelineContext?.PipelineBatchContext?.CurrentPipelineBatch;
      if (currentPipelineBatch == null)
        return;
      PipelineBatchSummary pipelineBatchSummary = currentPipelineBatch != null ? currentPipelineBatch.GetPipelineBatchSummary() : (PipelineBatchSummary) null;
      if (operations != null && pipelineBatchSummary != null)
        pipelineBatchSummary.EntitySubmitedCount += operations.Count<IXdbOperation>((Func<IXdbOperation, bool>) (o => o.Status == XdbOperationStatus.Succeeded || o.Status == XdbOperationStatus.Created));
      if (!this.VerificationLogEnabled(pipelineContext.PipelineBatchContext.CurrentPipelineBatch))
        return;
      if (operations != null)
        this.SetVerificationLogStatuses(pipelineContext, operations);
      this.SubmitVerificationLogEntries(pipelineContext);
    }

    protected virtual void SubmitVerificationLogEntries(PipelineContext pipelineContext)
    {
      List<IVerificationLogEntry> verificationLogCollection = this.GetVerificationLogCollection(pipelineContext);
      IVerificationLog verificationLog = this.GetVerificationLog(pipelineContext.PipelineBatchContext.CurrentPipelineBatch);
      foreach (IVerificationLogEntry verificationLogEntry1 in verificationLogCollection)
      {
        if (verificationLogEntry1 != null && verificationLogEntry1.TargetObject != null && verificationLogEntry1.TargetObject.GetType().IsSubclassOf(typeof (Entity)))
        {
          Entity targetObject = (Entity) verificationLogEntry1.TargetObject;
          if (targetObject != null)
          {
            // ISSUE: explicit non-virtual call
            Guid? id = targetObject.Id;
            if (id.HasValue)
            {
              IVerificationLogEntry verificationLogEntry2 = verificationLogEntry1;
              id = targetObject.Id;
              string str = id.ToString();
              verificationLogEntry2.TargetIdentifier = str;
            }
          }
        }
      }
      verificationLog.Add((ICollection<IVerificationLogEntry>) verificationLogCollection);
      verificationLogCollection.Clear();
    }

    protected virtual bool VerificationLogEnabled(PipelineBatch pipelineBatch)
    {
      VerificationLogSettings verificationLogSettings = this.GetVerificationLogSettings(pipelineBatch);
      return verificationLogSettings != null && verificationLogSettings.VerificationEnabled;
    }

    protected virtual IVerificationLog GetVerificationLog(
      PipelineBatch pipelineBatch)
    {
      return this.GetVerificationLogSettings(pipelineBatch)?.VerificationLog;
    }

    protected VerificationLogSettings GetVerificationLogSettings(
      PipelineBatch pipelineBatch)
    {
      return pipelineBatch.GetPlugin<VerificationLogSettings>();
    }

    private void SetVerificationLogStatuses(
      PipelineContext pipelineContext,
      ICollection<IXdbOperation> operations)
    {
      foreach (IVerificationLogEntry verificationLog in this.GetVerificationLogCollection(pipelineContext))
      {
        IVerificationLogEntry entry = verificationLog;
        if (entry?.TargetObject != null && entry.TargetObject.GetType().IsSubclassOf(typeof (Entity)))
        {
          IXdbOperation xdbOperation = operations.FirstOrDefault<IXdbOperation>((Func<IXdbOperation, bool>) (o => o.Target.Equals(entry.TargetObject)));
          if (xdbOperation != null)
          {
            entry.OperationType = xdbOperation.OperationType.ToString("G");
            entry.Status = xdbOperation.Status.ToString("G");
          }
        }
      }
    }
  }
}
