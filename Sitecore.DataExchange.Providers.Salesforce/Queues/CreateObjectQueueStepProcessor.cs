// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.CreateObjectQueueStepProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (BatchEntryStorageSettings)})]
  public class CreateObjectQueueStepProcessor : BasePipelineStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IHasPlugins entryStorageLocation = this.GetBatchEntryStorageLocation(pipelineStep, pipelineContext);
      if (entryStorageLocation == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "No value was specified for the location where the entity queue plugin should be stored.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
      }
      else
      {
        ObjectQueuePlugin entityQueuePlugin = this.CreateEntityQueuePlugin(pipelineStep, pipelineContext, logger);
        if (entityQueuePlugin == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "No entity queue plugin was resolved.", Array.Empty<string>());
          pipelineContext.CriticalError = true;
        }
        else
          entryStorageLocation.AddPlugin<ObjectQueuePlugin>(entityQueuePlugin);
      }
    }

    protected virtual Guid GetBatchEntryStoragePluginLocation(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<BatchEntryStorageSettings>().PluginLocation;
    }

    protected virtual ObjectQueuePlugin CreateEntityQueuePlugin(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      int minimumBatchSize = pipelineStep.GetPlugin<BatchEntryStorageSettings>().DefaultMinimumBatchSize;
      Endpoint endpointTo = pipelineStep?.GetPlugin<EndpointSettings>()?.EndpointTo;
      return new ObjectQueuePlugin()
      {
        MinimumBatchSize = minimumBatchSize,
        Endpoint = endpointTo
      };
    }
  }
}
