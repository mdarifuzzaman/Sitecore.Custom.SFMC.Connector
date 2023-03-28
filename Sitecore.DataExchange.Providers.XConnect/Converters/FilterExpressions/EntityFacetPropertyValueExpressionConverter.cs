// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.FilterExpressions.EntityFacetPropertyValueExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Framework.Conditions;
using Sitecore.Services.Core.Model;
using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.FilterExpressions
{
  [SupportedIds(new string[] {"{BA53E604-82F3-4652-9207-A4602D554280}"})]
  public class EntityFacetPropertyValueExpressionConverter : 
    BaseItemModelConverter<IEntityExpressionBuilder>
  {
    public const string FieldNameFacetProperty = "FacetProperty";
    public const string FieldNamePropertyName = "PropertyName";
    public const string FieldNameReadOnly = "ReadOnly";
    public const string FieldNameFacetType = "FacetType";
    public const string FieldNameConditionOperator = "ConditionOperator";
    protected readonly ExpressionTypeConverter ExpressionTypeConverter;

    public EntityFacetPropertyValueExpressionConverter(IItemModelRepository repository)
      : this(repository, new ExpressionTypeConverter())
    {
    }

    public EntityFacetPropertyValueExpressionConverter(
      IItemModelRepository repository,
      ExpressionTypeConverter expressionTypeConverter)
      : base(repository)
    {
      this.ExpressionTypeConverter = expressionTypeConverter ?? new ExpressionTypeConverter();
    }

    protected override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      Condition.Requires<ItemModel>(source).IsNotNull<ItemModel>();
      ItemModel referenceAsModel = this.GetReferenceAsModel(source, "FacetProperty");
      if (referenceAsModel == null)
        return this.NegativeResult(source, "No facet property item is referenced.");
      Type facetType = this.GetFacetType(referenceAsModel);
      string stringValue = this.GetStringValue(referenceAsModel, "PropertyName");
      ConvertResult<ExpressionType> convertResult = this.ExpressionTypeConverter.Convert(this.GetConditionOperator(source));
      if (!convertResult.WasConverted)
        return this.NegativeResult(source, "Cannot convert condition operator to expression type. (Reason: " + convertResult.Reason + ")");
      ExpressionType convertedValue = convertResult.ConvertedValue;
      return this.PositiveResult((IEntityExpressionBuilder) new EntityFacetPropertyValueExpressionBuilder(facetType, stringValue, convertedValue));
    }

    protected virtual Guid GetConditionOperator(ItemModel source) => this.GetGuidValue(source, "ConditionOperator");

    protected virtual bool TryGetExpressionType(
      string conditionOperator,
      out ExpressionType expressionType)
    {
      return Enum.TryParse<ExpressionType>(conditionOperator, true, out expressionType);
    }

    protected virtual Type GetFacetType(ItemModel facetPropertyItem)
    {
      if (!facetPropertyItem.ContainsKey("ParentID"))
        return (Type) null;
      ItemModel referenceAsModel = this.GetReferenceAsModel(facetPropertyItem, "ParentID");
      return referenceAsModel == null ? (Type) null : this.GetTypeFromTypeName(referenceAsModel, "FacetType");
    }
  }
}
