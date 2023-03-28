// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.ValueMatchesConstantConditionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  [SupportedIds(new string[] {"{AA9A6ACD-28C4-45B0-BC60-347B582D2EA5}"})]
  public class ValueMatchesConstantConditionConverter : BaseXObjectMappingConditionConverter
  {
    public const string FieldNameConstantValue = "ConstantValue";
    public const string FieldNameConstantValueType = "ConstantValueType";
    public const string ReferencedFieldNameTypeName = "TypeName";

    public ValueMatchesConstantConditionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IXObjectMappingCondition> ConvertSupportedItem(
      ItemModel source)
    {
      Type typeFromReference = this.GetTypeFromReference(source, "ConstantValueType", "TypeName");
      if (typeFromReference == (Type) null)
        return this.NegativeResult(source, "The referenced field value was not converted into a type.");
      object convertedToValueType = this.GetStringValueConvertedToValueType(source, "ConstantValue", typeFromReference);
      if (convertedToValueType == null)
        return this.NegativeResult(source, "The string value could not be converted into the specified type.", string.Format("value: {0}", convertedToValueType), "type: " + typeFromReference.FullName);
      IValueAccessor sourceValueAccessor = this.GetSourceValueAccessor(source);
      ValueMatchesConstantCondition constantCondition = new ValueMatchesConstantCondition();
      constantCondition.ConstantValue = convertedToValueType;
      constantCondition.SourceValueAccessor = sourceValueAccessor;
      return this.PositiveResult((IXObjectMappingCondition) constantCondition);
    }
  }
}
