// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.AddXConnectClientToContextStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (EndpointSettings), typeof (BatchEntryStorageSettings), typeof (BatchProcessingSettings)})]
  [RequiredEndpointPlugins(new Type[] {typeof (XConnectClientEndpointSettings)})]
  public class AddXConnectClientToContextStepProcessor : BasePipelineStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IHasPlugins entryStorageLocation = this.GetBatchEntryStorageLocation(pipelineStep, pipelineContext);
      if (entryStorageLocation == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the location of the batch object from the pipeline step.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
      }
      else
      {
        XConnectClientPlugin xconnectClientPlugin = this.GetXConnectClientPlugin(pipelineStep, pipelineContext, logger);
        if (xconnectClientPlugin == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the xConnect client plugin.", Array.Empty<string>());
          pipelineContext.CriticalError = true;
        }
        else
          entryStorageLocation.AddPlugin<XConnectClientPlugin>(xconnectClientPlugin);
      }
    }

    protected virtual XConnectClientPlugin GetXConnectClientPlugin(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      EndpointSettings endpointSettings = pipelineStep.GetEndpointSettings();
      if (endpointSettings == null)
        return (XConnectClientPlugin) null;
      if (endpointSettings.EndpointTo == null)
        return (XConnectClientPlugin) null;
      XConnectClientEndpointSettings plugin1 = endpointSettings.EndpointTo.GetPlugin<XConnectClientEndpointSettings>();
      if (plugin1 == null)
        return (XConnectClientPlugin) null;
      BatchProcessingSettings plugin2 = pipelineStep.GetPlugin<BatchProcessingSettings>();
      return new XConnectClientPlugin(plugin1.ClientSettings, plugin1.ClientHelper)
      {
        MinimumBatchSize = plugin2.MinimumBatchSize
      };
    }
  }
}
