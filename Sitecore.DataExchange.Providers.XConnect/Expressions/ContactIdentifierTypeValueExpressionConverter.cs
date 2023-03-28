// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.ContactIdentifierTypeValueExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class ContactIdentifierTypeValueExpressionConverter : 
    BaseItemModelConverter<IEntityExpressionBuilder>
  {
    public const string FieldNameConditionOperator = "ConditionOperator";
    public const string FieldNameValue = "Value";
    public const string FieldNameRightValueAccessor = "RightValueAccessor";

    public ContactIdentifierTypeValueExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      string conditionOperator = this.GetConditionOperator(source);
      ExpressionType expressionType;
      if (!this.TryGetExpressionType(conditionOperator, out expressionType))
        return this.NegativeResult(source, "Cannot convert string condition operator to expression type. (condition operator: " + conditionOperator + ")");
      ContactIdentifierType contactIdentifierType = this.GetContactIdentifierType(source);
      IValueAccessor rightValueAccessor = this.GetRightValueAccessor(source);
      ContactIdentifierTypeValueExpressionBuilder expressionBuilder = new ContactIdentifierTypeValueExpressionBuilder();
      expressionBuilder.ContactIdentifierType = contactIdentifierType;
      expressionBuilder.ExpressionType = expressionType;
      expressionBuilder.RightValueAccessor = rightValueAccessor;
      return this.PositiveResult((IEntityExpressionBuilder) expressionBuilder);
    }

    protected virtual IValueAccessor GetRightValueAccessor(ItemModel source) => this.ConvertReferenceToModel<IValueAccessor>(source, "RightValueAccessor");

    protected virtual ContactIdentifierType GetContactIdentifierType(
      ItemModel source)
    {
      return this.GetEnumValue<ContactIdentifierType>(source, "Value");
    }

    protected virtual string GetConditionOperator(ItemModel source) => this.GetReferenceAsModel(source, "ConditionOperator")["ItemName"].ToString();

    protected virtual bool TryGetExpressionType(
      string conditionOperator,
      out ExpressionType expressionType)
    {
      return Enum.TryParse<ExpressionType>(conditionOperator, true, out expressionType);
    }
  }
}
