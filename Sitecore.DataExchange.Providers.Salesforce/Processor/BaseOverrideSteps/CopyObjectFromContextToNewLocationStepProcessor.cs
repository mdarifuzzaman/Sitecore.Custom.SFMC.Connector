using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.Processor.BaseOverrideSteps
{
    [RequiredPipelineStepPlugins(new Type[] { typeof(ContextLocationSettings) })]
    public class CopyObjectFromContextToNewLocationStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(
          PipelineStep pipelineStep,
          PipelineContext pipelineContext,
          ILogger logger)
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
            Guid targetObjectLocation = plugin.TargetObjectLocation;
            this.SetObjectOnPipelineContext(source, targetObjectLocation, pipelineContext, logger);
        }
    }
}
