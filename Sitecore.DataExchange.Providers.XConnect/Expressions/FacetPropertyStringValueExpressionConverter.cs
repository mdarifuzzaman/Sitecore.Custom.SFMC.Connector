// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.FacetPropertyStringValueExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  [SupportedIds(new string[] {"{B0002399-17BE-4ECF-8B8B-C1765ABB6F73}"})]
  public class FacetPropertyStringValueExpressionConverter : 
    BaseFacetPropertyValueExpressionConverter
  {
    public const string FieldNameValue = "Value";
    public const string FieldNameRightValueAccessor = "RightValueAccessor";
    public const string FieldNameUseEmptyStringValue = "UseEmptyStringForValue";
    public const string FieldNameUseNullObjectForValue = "UseNullObjectForValue";

    public FacetPropertyStringValueExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    public FacetPropertyStringValueExpressionConverter(
      IItemModelRepository repository,
      ExpressionTypeConverter expressionTypeConverter)
      : base(repository, expressionTypeConverter)
    {
    }

    protected override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      ConvertResult<ExpressionType> convertResult = this.ExpressionTypeConverter.Convert(this.GetConditionOperator(source));
      return !convertResult.WasConverted ? this.NegativeResult(source, "Cannot convert string condition expression to expression type.") : this.PositiveResult((IEntityExpressionBuilder) new FacetPropertyStringValueExpressionBuilder(convertResult.ConvertedValue, this.GetFacetType(source), this.GetFacetKeyName(source), this.GetFacetPropertyName(source), this.ConvertReferenceToModel<IValueAccessor>(source, "RightValueAccessor"), this.GetStringValue(source, "Value"), this.GetBoolValue(source, "UseNullObjectForValue"), this.GetBoolValue(source, "UseEmptyStringForValue")));
    }
  }
}
