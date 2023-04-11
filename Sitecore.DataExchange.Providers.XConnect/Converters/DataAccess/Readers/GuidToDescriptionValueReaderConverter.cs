using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers
{
    [SupportedIds(new string[] { "{4E0076F0-585E-462E-BD73-706B70738F83}" })]
    public class GuidToDescriptionValueReaderConverter : BaseItemModelConverter<IValueReader>
    {
        public const string FieldNameCodeDefinitionSet = "CodeDefinitionSet";
        public const string FieldNameCodeDefinitionValue = "Value";

        public GuidToDescriptionValueReaderConverter(IItemModelRepository repository)
          : base(repository)
        {
        }

        protected override ConvertResult<IValueReader> ConvertSupportedItem(
          ItemModel source)
        {
            ItemModel referenceAsModel = this.GetReferenceAsModel(source, "CodeDefinitionSet");
            if (referenceAsModel == null)
                return this.NegativeResult(source, "The field does not reference a valid item.", "field: CodeDefinitionSet");
            IEnumerable<ItemModel> childItemModels = this.GetChildItemModels(referenceAsModel);
            GuidToDescriptionValueReader descriptionValueReader = new GuidToDescriptionValueReader();
            foreach (ItemModel itemModel in childItemModels)
            {
                var value = this.GetStringValue(itemModel, "Value");
                descriptionValueReader.Values[value] = itemModel["ItemName"].ToString();
            }
            return this.PositiveResult((IValueReader)descriptionValueReader);
        }
    }

    public class GuidToDescriptionValueReader : IValueReader
    {
        public GuidToDescriptionValueReader() => this.Values = (IDictionary<string, string>)new Dictionary<string, string>();

        public IDictionary<string, string> Values { get; private set; }

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            var key = "{" + source.ToString().ToUpper() + "}";
            return !this.Values.ContainsKey(key) ? ReadResult.PositiveResult((object)null, DateTime.Now) : ReadResult.PositiveResult((object)this.Values[key], DateTime.Now);
        }
    }
}
