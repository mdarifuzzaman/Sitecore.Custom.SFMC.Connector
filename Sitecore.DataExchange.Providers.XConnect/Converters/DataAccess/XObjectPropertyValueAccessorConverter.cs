// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.XObjectPropertyValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{41558098-804D-42D6-831C-7308C93EB69D}"})]
  public class XObjectPropertyValueAccessorConverter : ValueAccessorConverter
  {
    public const string PropertyNameField = "PropertyName";

    public XObjectPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueAccessor> ConvertSupportedItem(
      ItemModel source)
    {
      string stringValue = this.GetStringValue(source, "PropertyName");
      ConvertResult<IValueAccessor> convertResult = base.ConvertSupportedItem(source);
      if (!convertResult.WasConverted)
        return convertResult;
      if (convertResult.ConvertedValue == null)
        return this.NegativeResult(source, "The referenced value accessor was converted into a null object.");
      IValueAccessor convertedValue = convertResult.ConvertedValue;
      if (convertedValue.ValueReader == null)
        convertedValue.ValueReader = (IValueReader) new IndexerPropertyValueReader(new object[1]
        {
          (object) stringValue
        });
      if (convertedValue.ValueWriter == null)
        convertedValue.ValueWriter = (IValueWriter) new IndexerPropertyValueWriter(new object[1]
        {
          (object) stringValue
        });
      return this.PositiveResult(convertedValue);
    }
  }
}
