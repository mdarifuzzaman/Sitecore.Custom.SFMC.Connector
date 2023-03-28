// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.NullValueConditionExpressionConverter
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
  [SupportedIds(new string[] {"{B71CB5FE-98C9-41C8-892E-3C0F0B1FB142}"})]
  public class NullValueConditionExpressionConverter : BaseConditionExpressionConverter
  {
    public const string FieldNameOperator = "Operator";

    public NullValueConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source)
    {
      return (Func<IValueAccessor, object, object[]>) ((va, obj) => (object[]) new string[1]
      {
        "null"
      });
    }

    protected override ConvertResult<ConditionExpressionDescriptor> ConvertSupportedItem(
      ItemModel source)
    {
      string fieldName = this.GetFieldName(source);
      string conditionOperator = this.GetConditionOperator(source, "Operator");
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
