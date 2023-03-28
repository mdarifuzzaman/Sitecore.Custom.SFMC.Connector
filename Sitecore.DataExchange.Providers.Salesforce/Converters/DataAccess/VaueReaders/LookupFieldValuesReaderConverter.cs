// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.VaueReaders.LookupFieldValuesReaderConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.VaueReaders
{
  [SupportedIds(new string[] {"{A842A9BA-CFAE-48B9-838B-5483439B208F}"})]
  public class LookupFieldValuesReaderConverter : BaseItemModelConverter<IValueReader>
  {
    public const string FieldNameFieldName = "FieldName";

    public LookupFieldValuesReaderConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueReader> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IValueReader) new LookupFieldValuesReader(this.GetStringValue(source, "FieldName")));
    }
  }
}
