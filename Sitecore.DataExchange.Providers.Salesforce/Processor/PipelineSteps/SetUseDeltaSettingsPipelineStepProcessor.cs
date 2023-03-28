using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.Processor.PipelineSteps
{
    [RequiredPipelineStepPlugins(new Type[] { typeof(UseDeltaSettings) })]
    public class SetUseDeltaSettingsPipelineStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(
          PipelineStep pipelineStep,
          PipelineContext pipelineContext,
          ILogger logger)
        {
            UseDeltaSettings useDeltaSettings = pipelineStep.GetUseDeltaSettings();
            if (useDeltaSettings == null)
                return;
            DateTime? nullable = this.ReadDateTime(useDeltaSettings, pipelineContext, logger);
            if (!nullable.HasValue)
                return;
            DateRangeSettings newPlugin = new DateRangeSettingsHelper().Create(nullable.Value, useDeltaSettings.Operator, useDeltaSettings.Offset, useDeltaSettings.UnitOfTime);
            pipelineContext.AddPlugin<DateRangeSettings>(newPlugin);
        }

        private DateTime? ReadDateTime(
          UseDeltaSettings deltaSettings,
          PipelineContext pipelineContext,
          ILogger logger)
        {
            if (deltaSettings != null && deltaSettings.ContextValueReader != null)
            {
                DataAccessContext context = new DataAccessContext();
                //var test = deltaSettings.ContextValueReader as DataExchange.DataAccess.Readers.SequentialValueReader;
                //if(test != null)
                //{
                //    int num2 = 0;
                //    int num1 = test.Readers.Count;
                //    object source1 = pipelineContext;
                //    DataExchange.DataAccess.Plugins.SequentialValueReaderSettings valueReaderSettings = new DataExchange.DataAccess.Plugins.SequentialValueReaderSettings()
                //    {
                //        OriginalContext = context,
                //        OriginalSource = pipelineContext
                //    };
                //    DataAccessContext context1 = new DataAccessContext();
                //    context1.Plugins.Add((IDataAccessPlugin)valueReaderSettings);
                //    bool flag = false;
                //    object obj = (object)null;
                //    foreach (var reader in test.Readers)
                //    {
                //        ++num2;
                //        ReadResult readResult1 = reader.Read(source1, context1);
                //        if (!readResult1.WasValueRead)
                //            source1 = (object)null;
                //        else if (num2 == num1)
                //            flag = true;
                //        source1 = readResult1.ReadValue;
                //        if (source1 == null)
                //            break;
                //    }
                //    obj = source1;
                //    var finalReadResult = new ReadResult(DateTime.UtcNow)
                //    {
                //        WasValueRead = flag,
                //        ReadValue = obj
                //    };
                //}

                ReadResult readResult = deltaSettings.ContextValueReader.Read((object)pipelineContext, context);

                if (readResult.WasValueRead && readResult.ReadValue is DateTime)
                    return new DateTime?((DateTime)readResult.ReadValue);
            }
            return new DateTime?();
        }
    }
}
