// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.BooleanConditionExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  [SupportedIds(new string[] {"{C23C4B0A-7A45-4542-9AA0-CE759DA89C86}"})]
  public class BooleanConditionExpressionConverter : BaseConditionExpressionConverter
  {
    public const string FieldNameIs = "Is";
    public const string ConditionIsOperator = "IsOperator";

    public BooleanConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source)
    {
      return (Func<IValueAccessor, object, object[]>) ((va, obj) => new object[1]
      {
        (object) true
      });
    }

    protected override ConvertResult<ConditionExpressionDescriptor> ConvertSupportedItem(
      ItemModel source)
    {
      bool boolValue = this.GetBoolValue(source, "Is");
      source.Add("IsOperator", (object) (boolValue ? Sitecore.DataExchange.ItemIDs.BooleanConditionOperatorsIs : Sitecore.DataExchange.ItemIDs.BooleanConditionOperatorsIsNot));
      string fieldName = this.GetFieldName(source);
      string conditionOperator = this.GetConditionOperator(source, "IsOperator");
      Func<IValueAccessor, object, object[]> delegateForGetValues = this.GetDelegateForGetValues(source);
      return this.PositiveResult(new ConditionExpressionDescriptor()
      {
        FieldName = fieldName,
        Operator = conditionOperator,
        GetValues = delegateForGetValues
      });
    }
  }
}
