using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.Processor.PipelineSteps
{
    [SupportedIds(new string[] { "{E2F11AE2-6C89-411C-879F-9FFBCAB98C90}" })]
    public class SetUseDeltaSettingsPipelineStepConverter : BasePipelineStepConverter
    {
        public const string FieldNameContextValueReader = "ContextValueReader";

        public SetUseDeltaSettingsPipelineStepConverter(IItemModelRepository repository)
          : base(repository)
        {
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            UseDeltaSettings newPlugin = new UseDeltaSettings()
            {
                Offset = this.GetIntValue(source, "Offset"),
                Operator = this.GetEnumValue<DateOffsetOperator>(source, "Operator"),
                UnitOfTime = this.GetEnumValue<UnitOfTime>(source, "UnitOfTime"),
                ContextValueReader = this.ConvertReferenceToModel<IValueReader>(source, "ContextValueReader")
            };
            pipelineStep.AddPlugin<UseDeltaSettings>(newPlugin);
        }
    }
}
