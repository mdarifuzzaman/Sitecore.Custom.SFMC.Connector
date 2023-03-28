// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.AddXdbReferenceDataClientToContextStepProcessor
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
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Xdb.ReferenceData.Core;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (EndpointSettings), typeof (BatchEntryStorageSettings), typeof (BatchProcessingSettings), typeof (ReferenceDataDefinitionSettings)})]
  public class AddXdbReferenceDataClientToContextStepProcessor : BasePipelineStepProcessor
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
        ReferenceDataClientPlugin dataClientPlugin = this.GetReferenceDataClientPlugin(pipelineStep, pipelineContext, logger);
        if (dataClientPlugin == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the reference data client plugin.", Array.Empty<string>());
          pipelineContext.CriticalError = true;
        }
        else
        {
          DefinitionTypeKey definitionTypeKey = this.GetDefinitionTypeKey(dataClientPlugin, pipelineStep, pipelineContext, logger);
          if (definitionTypeKey == null)
            return;
          dataClientPlugin.DefinitionTypeKey = definitionTypeKey;
          entryStorageLocation.AddPlugin<ReferenceDataClientPlugin>(dataClientPlugin);
        }
      }
    }

    protected virtual DefinitionTypeKey GetDefinitionTypeKey(
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IReferenceDataClient client = plugin.GetClient(0);
      if (client == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the client.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (DefinitionTypeKey) null;
      }
      ReferenceDataDefinitionSettings plugin1 = pipelineStep.GetPlugin<ReferenceDataDefinitionSettings>();
      DefinitionTypeKey definitionTypeKey = client.EnsureDefinitionType(plugin1.DefinitionTypeName);
      if (definitionTypeKey != null)
        return definitionTypeKey;
      this.Log(new Action<string>(logger.Error), pipelineContext, "Unable definition type.", new string[1]
      {
        "definition type name: " + plugin1.DefinitionTypeName
      });
      pipelineContext.CriticalError = true;
      return (DefinitionTypeKey) null;
    }

    protected virtual ReferenceDataClientPlugin GetReferenceDataClientPlugin(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      EndpointSettings endpointSettings = pipelineStep.GetEndpointSettings();
      if (endpointSettings.EndpointTo == null)
        return (ReferenceDataClientPlugin) null;
      BatchProcessingSettings plugin1 = pipelineStep.GetPlugin<BatchProcessingSettings>();
      ReferenceDataClientEndpointSettings plugin2 = endpointSettings.EndpointTo.GetPlugin<ReferenceDataClientEndpointSettings>();
      if (plugin2 == null)
        return (ReferenceDataClientPlugin) null;
      ReferenceDataDefinitionSettings plugin3 = pipelineStep.GetPlugin<ReferenceDataDefinitionSettings>();
      return new ReferenceDataClientPlugin(plugin2.BaseAddress, plugin3.CommonDataType, plugin3.CultureDataType)
      {
        Modifiers = plugin2.Modifiers,
        DefinitionTypeVersion = plugin3.DefinitionTypeVersion,
        MinimumBatchSize = plugin1.MinimumBatchSize
      };
    }
  }
}
