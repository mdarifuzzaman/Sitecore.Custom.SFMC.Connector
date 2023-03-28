// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ReadMessageItemFromEmailSentEventProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class ReadMessageItemFromEmailSentEventProcessor : BasePipelineStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep = null,
      PipelineContext pipelineContext = null,
      ILogger logger = null)
    {
      ContextLocationSettings plugin = pipelineStep.GetPlugin<ContextLocationSettings>();
      object source = this.GetObjectFromPipelineContext(plugin.SourceObjectLocation, pipelineContext, logger);
      if (plugin.SourceObjectValueAccessor != null)
      {
        IValueReader valueReader = plugin.SourceObjectValueAccessor.ValueReader;
        if (valueReader == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Pipeline step processing will abort because no value reader is set on the source object value accessor.", Array.Empty<string>());
          return;
        }
        ReadResult readResult = valueReader.Read(source, new DataAccessContext());
        if (!readResult.WasValueRead)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Pipeline step processing will abort because the value reader could not read a value from the source object value accessor.", Array.Empty<string>());
          return;
        }
        source = readResult.ReadValue;
      }
      if (source == null)
      {
        pipelineContext.Finished = true;
        this.Log(new Action<string>(logger.Warn), pipelineContext, "The message item is null.", Array.Empty<string>());
      }
      else
      {
        Guid targetObjectLocation = plugin.TargetObjectLocation;
        this.SetObjectOnPipelineContext(source, targetObjectLocation, pipelineContext, logger);
      }
    }
  }
}
