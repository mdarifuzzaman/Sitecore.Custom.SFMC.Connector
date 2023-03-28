using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.VaueReaders
{
    public class LookupFieldValueReaderConverter : BaseItemModelConverter<IValueReader>
    {
        public const string FieldNameFieldName = "FieldName";

        public LookupFieldValueReaderConverter(IItemModelRepository repository)
          : base(repository)
        {
        }

        protected override ConvertResult<IValueReader> ConvertSupportedItem(
          ItemModel source)
        {
            return this.PositiveResult((IValueReader)new LookupFieldValueReader(this.GetStringValue(source, "FieldName")));
        }
    }
}
