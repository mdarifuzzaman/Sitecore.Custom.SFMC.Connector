// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.ValueAccessors.ObjectFieldValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers;
using Sitecore.DataExchange.Providers.Salesforce.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.ValueAccessors
{
  [SupportedIds(new string[] {"{1B872924-E270-4E66-8CD6-1615494E3FB3}"})]
  public class ObjectFieldValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameFieldName = "FieldName";
    public const string FieldNameFieldNameForSelect = "FieldNameForSelect";

    public ObjectFieldValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueAccessor> ConvertSupportedItem(
      ItemModel source)
    {
      ConvertResult<IValueAccessor> convertResult = base.ConvertSupportedItem(source);
      if (!convertResult.WasConverted)
        return convertResult;
      IValueAccessor convertedValue = convertResult.ConvertedValue;
      if (convertedValue == null)
        return this.NegativeResult(source, "The source item was converted into a null object.");
      string stringValue = this.GetStringValue(source, "FieldName");
      if (convertedValue.ValueReader == null)
      {
        ObjectFieldValueReader fieldValueReader = new ObjectFieldValueReader(stringValue);
        convertedValue.ValueReader = (IValueReader) fieldValueReader;
      }
      if (convertedValue.ValueWriter == null)
        convertedValue.ValueWriter = (IValueWriter) new ExpandoObjectPropertyValueWriter(new object[1]
        {
          (object) stringValue
        });
      return this.PositiveResult(convertedValue);
    }
  }
}
