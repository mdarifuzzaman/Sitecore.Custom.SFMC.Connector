// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.BaseSubmitXdbReferenceDataBatchStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Helpers;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Xdb.ReferenceData.Core;
using Sitecore.Xdb.ReferenceData.Core.Results;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  public abstract class BaseSubmitXdbReferenceDataBatchStepProcessor : BasePipelineStepProcessor
  {
    public RetryHelper RetryHelper { get; set; } = new RetryHelper();

    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ReferenceDataClientPlugin plugin = pipelineContext.GetPlugin<ReferenceDataClientPlugin>(true);
      if (this.GetReferenceDataClient(plugin, pipelineStep, pipelineContext, logger) == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get reference data client from the plugin.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
      }
      else
        this.Process(plugin, pipelineStep, pipelineContext, logger);
    }

    protected abstract void Process(
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected virtual void SubmitBatch(
      int threadId,
      IList<BaseDefinition> batch,
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IReferenceDataClient client = plugin.GetClient(threadId);
      if (client == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get client for thread.", new string[1]
        {
          string.Format("thread id: {0}", (object) threadId)
        });
        pipelineContext.CriticalError = true;
      }
      else
      {
        ReferenceDataHelperContext context = new ReferenceDataHelperContext()
        {
          Client = client,
          CommonDataType = plugin.CommonDataType,
          CultureDataType = plugin.CultureDataType
        };
        SaveDefinitionsBatchResult result = plugin.ReferenceDataHelper.SubmitDefinitions(batch, context, logger);
        if (result == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to submit definitions.", new string[2]
          {
            string.Format("thread id: {0}", (object) threadId),
            string.Format("batch size: {0}", (object) batch.Count)
          });
          pipelineContext.CriticalError = true;
        }
        else
          this.HandleSubmitResults(result, batch, threadId, pipelineStep, pipelineContext, logger);
      }
    }

    protected virtual IReferenceDataClient GetReferenceDataClient(
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      int? threadId = this.GetThreadId(pipelineContext);
      return plugin.GetClient(!threadId.HasValue ? 0 : threadId.Value);
    }

    protected virtual void HandleSubmitResults(
      SaveDefinitionsBatchResult result,
      IList<BaseDefinition> batch,
      int threadId,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (result == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Definition batch was not submitted.", new string[2]
        {
          string.Format("thread id: {0}", (object) threadId),
          string.Format("batch size: {0}", (object) batch.Count)
        });
        pipelineContext.CriticalError = true;
      }
      else if (!result.Success)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Submitting definition batch was not successful.", new string[2]
        {
          string.Format("thread id: {0}", (object) threadId),
          string.Format("batch size: {0}", (object) batch.Count)
        });
        pipelineContext.CriticalError = true;
      }
      else
      {
        this.Log(new Action<string>(logger.Info), pipelineContext, "Definition batch was successfully submitted.", new string[2]
        {
          string.Format("thread id: {0}", (object) threadId),
          string.Format("batch size: {0}", (object) batch.Count)
        });
        this.Log(new Action<string>(logger.Debug), pipelineContext, "Clearing definition batch.", new string[2]
        {
          string.Format("thread id: {0}", (object) threadId),
          string.Format("batch size: {0}", (object) batch.Count)
        });
        batch.Clear();
      }
    }
  }
}
