// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.SubmitXdbReferenceDataBatchStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Xdb.ReferenceData.Core;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [RequiredPipelineContextPlugins(new Type[] {typeof (ReferenceDataClientPlugin)})]
  public class SubmitXdbReferenceDataBatchStepProcessor : 
    BaseSubmitXdbReferenceDataBatchStepProcessor
  {
    protected override void Process(
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      int? nullable = this.GetThreadId(pipelineContext);
      if (!nullable.HasValue)
        nullable = new int?(0);
      IList<BaseDefinition> batch = plugin.GetBatch(nullable.Value);
      if (batch == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get batch from the plugin.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
      }
      else
      {
        if (batch.Count == 0)
          return;
        this.SubmitBatch(nullable.Value, batch, plugin, pipelineStep, pipelineContext, logger);
      }
    }
  }
}
