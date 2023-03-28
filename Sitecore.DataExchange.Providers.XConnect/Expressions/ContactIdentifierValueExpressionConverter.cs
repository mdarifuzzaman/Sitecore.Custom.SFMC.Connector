// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.ContactIdentifierValueExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class ContactIdentifierValueExpressionConverter : 
    BaseItemModelConverter<IEntityExpressionBuilder>
  {
    public const string FieldNameConditionOperator = "ConditionOperator";
    public const string FieldNameRightValueAccessor = "RightValueAccessor";
    public const string FieldNameValue = "Value";
    public const string FieldNameUseNullObjectForValue = "UseNullObjectForValue";
    public const string FieldNameUseEmptyStringForValue = "UseEmptyStringForValue";
    protected readonly ExpressionTypeConverter ExpressionTypeConverter;

    public ContactIdentifierValueExpressionConverter(IItemModelRepository repository)
      : this(repository, new ExpressionTypeConverter())
    {
    }

    public ContactIdentifierValueExpressionConverter(
      IItemModelRepository repository,
      ExpressionTypeConverter expressionTypeConverter)
      : base(repository)
    {
      this.ExpressionTypeConverter = expressionTypeConverter ?? new ExpressionTypeConverter();
    }

    protected override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      ConvertResult<ExpressionType> convertResult = this.ExpressionTypeConverter.Convert(this.GetConditionOperator(source));
      if (!convertResult.WasConverted)
        return this.NegativeResult(source, "Cannot convert condition operator to expression type. (Reason: " + convertResult.Reason + ")");
      ExpressionType convertedValue = convertResult.ConvertedValue;
      IValueAccessor rightValueAccessor = this.GetRightValueAccessor(source);
      bool flag1 = this.ShouldUseEmptyStringForValue(source);
      bool flag2 = this.ShouldUseNullForValue(source);
      string str = this.GetValue(source);
      ContactIdentifierValueExpressionBuilder expressionBuilder = new ContactIdentifierValueExpressionBuilder();
      expressionBuilder.ExpressionType = convertedValue;
      expressionBuilder.RightValueAccessor = rightValueAccessor;
      expressionBuilder.Value = str;
      expressionBuilder.UseEmptyStringForValue = flag1;
      expressionBuilder.UseNullObjectForValue = flag2;
      return this.PositiveResult((IEntityExpressionBuilder) expressionBuilder);
    }

    protected virtual Guid GetConditionOperator(ItemModel source) => this.GetGuidValue(source, "ConditionOperator");

    protected virtual string GetValue(ItemModel source) => this.GetStringValue(source, "Value");

    protected virtual bool ShouldUseNullForValue(ItemModel source) => this.GetBoolValue(source, "UseNullObjectForValue");

    protected virtual IValueAccessor GetRightValueAccessor(ItemModel source) => this.ConvertReferenceToModel<IValueAccessor>(source, "RightValueAccessor");

    protected virtual bool ShouldUseEmptyStringForValue(ItemModel source) => this.GetBoolValue(source, "UseEmptyStringForValue");
  }
}
