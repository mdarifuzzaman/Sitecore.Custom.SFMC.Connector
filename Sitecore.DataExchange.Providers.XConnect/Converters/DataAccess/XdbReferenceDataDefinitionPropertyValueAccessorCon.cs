// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.XdbReferenceDataDefinitionPropertyValueAccessorConverter
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
  [SupportedIds(new string[] {"{0687CA5B-1D0A-4F44-8FBF-90CFFC2A5620}"})]
  public class XdbReferenceDataDefinitionPropertyValueAccessorConverter : ValueAccessorConverter
  {
    public const string PropertyNameFieldName = "PropertyName";
    public const string CultureSpecificValueFieldName = "CultureSpecificValue";

    public XdbReferenceDataDefinitionPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueAccessor> ConvertSupportedItem(
      ItemModel source)
    {
      string stringValue = this.GetStringValue(source, "PropertyName");
      PropertyValueReader propertyValueReader = new PropertyValueReader(this.GetBoolValue(source, "CultureSpecificValue") ? "CultureData" : "CommonData");
      ConvertResult<IValueAccessor> convertResult = base.ConvertSupportedItem(source);
      if (!convertResult.WasConverted)
        return convertResult;
      if (convertResult.ConvertedValue == null)
        return this.NegativeResult(source, "The referenced value accessor was converted into a null object.");
      IValueAccessor convertedValue = convertResult.ConvertedValue;
      if (convertedValue.ValueReader == null)
        convertedValue.ValueReader = (IValueReader) new SequentialValueReader()
        {
          Readers = {
            (IValueReader) propertyValueReader,
            (IValueReader) new PropertyValueReader(stringValue)
          }
        };
      if (convertedValue.ValueWriter == null)
        convertedValue.ValueWriter = (IValueWriter) new TargetReaderValueWriter()
        {
          ValueAccessor = (IValueAccessor) new ValueAccessor()
          {
            ValueReader = (IValueReader) propertyValueReader,
            ValueWriter = (IValueWriter) new PropertyValueWriter(stringValue)
          }
        };
      return this.PositiveResult(convertedValue);
    }
  }
}
