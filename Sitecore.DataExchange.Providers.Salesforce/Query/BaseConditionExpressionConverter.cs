// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.BaseConditionExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  public abstract class BaseConditionExpressionConverter : 
    BaseItemModelConverter<ConditionExpressionDescriptor>
  {
    public const string ReferencedFieldNameFieldName = "FieldName";
    public const string FieldNameLeftValueAccessor = "LeftValueAccessor";
    public const string FieldNameConditionOperator = "ConditionOperator";
    public const string FieldNameRightValueAccessor = "RightValueAccessor";
    private static IDictionary<Guid, string> _operators = (IDictionary<Guid, string>) new Dictionary<Guid, string>();

    static BaseConditionExpressionConverter()
    {
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NumericConditionOperatorsEqual] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NumericConditionOperatorsGreaterOrThanEqualTo] = ">=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NumericConditionOperatorsGreaterThan] = ">";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NumericConditionOperatorsLessThan] = "<";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NumericConditionOperatorsLessThanOrEqualTo] = "<=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NumericConditionOperatorsNotEqual] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ObjectConditionOperatorsEqual] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ObjectConditionOperatorsNotEqual] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.BooleanConditionOperatorsIs] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.BooleanConditionOperatorsIsNot] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsEqual] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsGreaterOrThanEqualTo] = ">=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsGreaterThan] = ">";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsLessThan] = "<";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsLessThanOrEqualTo] = "<=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsNotEqual] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.DateConditionOperatorsEqual] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.DateConditionOperatorsGreaterOrThanEqualTo] = ">=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.DateConditionOperatorsGreaterThan] = ">";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.DateConditionOperatorsLessThan] = "<";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.DateConditionOperatorsLessThanOrEqualTo] = "<=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.DateConditionOperatorsNotEqual] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NullConditionOperatorsIsNotNull] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.NullConditionOperatorsIsNull] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.StringConditionOperatorsEqual] = "=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.StringConditionOperatorsNotEqual] = "!=";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.StringConditionOperatorsStartsWith] = "LIKE";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.StringConditionOperatorsEndsWith] = "LIKE";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsIn] = "IN";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.ConditionOperatorsNotIn] = "NOT IN";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.StringConditionOperatorsContains] = "LIKE";
      BaseConditionExpressionConverter._operators[Sitecore.DataExchange.ItemIDs.StringConditionOperatorMatches] = "LIKE";
    }

    protected BaseConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<ConditionExpressionDescriptor> ConvertSupportedItem(
      ItemModel source)
    {
      string fieldName = this.GetFieldName(source);
      string conditionOperator = this.GetConditionOperator(source, "ConditionOperator");
      Func<IValueAccessor, object, object[]> delegateForGetValues = this.GetDelegateForGetValues(source);
      return this.PositiveResult(new ConditionExpressionDescriptor()
      {
        FieldName = fieldName,
        Operator = conditionOperator,
        GetValues = delegateForGetValues
      });
    }

    protected virtual IValueAccessor GetLeftValueAccessor(ItemModel source) => this.ConvertReferenceToModel<IValueAccessor>(source, "LeftValueAccessor");

    protected virtual IValueAccessor GetRightValueAccessor(ItemModel source) => this.ConvertReferenceToModel<IValueAccessor>(source, "RightValueAccessor");

    protected abstract Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source);

    protected virtual string GetFieldName(ItemModel source) => this.GetStringValueFromReference(source, "LeftValueAccessor", "FieldName");

    protected virtual string GetConditionOperator(ItemModel source, string fieldName)
    {
      Guid guidValue = this.GetGuidValue(source, fieldName);
      return BaseConditionExpressionConverter._operators.ContainsKey(guidValue) ? BaseConditionExpressionConverter._operators[guidValue] : (string) null;
    }
  }
}
