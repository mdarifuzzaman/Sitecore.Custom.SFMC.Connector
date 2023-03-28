// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.BaseFacetPropertyValueExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public abstract class BaseFacetPropertyValueExpressionConverter : 
    BaseItemModelConverter<IEntityExpressionBuilder>
  {
    public const string ReferenceFieldNameFacetName = "FacetName";
    public const string ReferenceFieldNameFacetType = "FacetType";
    public const string FieldNameFacetProperty = "FacetProperty";
    public const string FieldNameFacetSubProperty = "SubProperty";
    public const string FieldNameConditionOperator = "ConditionOperator";
    protected readonly ExpressionTypeConverter ExpressionTypeConverter;

    protected BaseFacetPropertyValueExpressionConverter(IItemModelRepository repository)
      : this(repository, new ExpressionTypeConverter())
    {
    }

    protected BaseFacetPropertyValueExpressionConverter(
      IItemModelRepository repository,
      ExpressionTypeConverter expressionTypeConverter)
      : base(repository)
    {
      this.ExpressionTypeConverter = expressionTypeConverter ?? new ExpressionTypeConverter();
    }

    protected virtual Guid GetConditionOperator(ItemModel source) => this.GetGuidValue(source, "ConditionOperator");

    protected virtual string GetFacetPropertyName(ItemModel source)
    {
      ItemModel referenceAsModel = this.GetReferenceAsModel(source, "FacetProperty");
      string stringValue = this.GetStringValue(source, "SubProperty");
      string str = referenceAsModel["ItemName"].ToString();
      if (!string.IsNullOrWhiteSpace(stringValue))
        str = str + "." + stringValue;
      return str.Trim(' ', '.', ',', '\t');
    }

    protected virtual string GetFacetKeyName(ItemModel source) => this.GetStringValue(this.GetParentItem(this.GetReferenceAsModel(source, "FacetProperty")), "FacetName");

    protected virtual Type GetFacetType(ItemModel source) => this.GetTypeFromTypeName(this.GetParentItem(this.GetReferenceAsModel(source, "FacetProperty")), "FacetType");

    protected abstract override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source);
  }
}
