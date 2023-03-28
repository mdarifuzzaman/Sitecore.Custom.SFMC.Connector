// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.SubmitXConnectBatchStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class SubmitXConnectBatchStepProcessor : BaseSubmitXConnectBatchStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientPlugin plugin = pipelineContext.GetPlugin<XConnectClientPlugin>(true);
      if (plugin == null)
        return;
      foreach (KeyValuePair<int, IXdbContext> client in (IEnumerable<KeyValuePair<int, IXdbContext>>) plugin.GetClients())
        this.SubmitBatch(string.Format("Batch from thread {0}", (object) client.Key), client.Value, pipelineStep, pipelineContext, logger);
    }
  }
}
