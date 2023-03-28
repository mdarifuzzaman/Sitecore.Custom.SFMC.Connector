// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.SubmitCurrentXConnectBatchStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class SubmitCurrentXConnectBatchStepProcessor : BaseSubmitXConnectBatchStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientPlugin plugin = pipelineContext.GetPlugin<XConnectClientPlugin>(true);
      if (plugin == null)
        return;
      int threadId = this.GetThreadId(pipelineContext) ?? 0;
      IXdbContext client = plugin.GetClient(threadId);
      this.SubmitBatch(string.Format("Batch from thread {0}", (object) threadId), client, pipelineStep, pipelineContext, logger);
    }
  }
}
