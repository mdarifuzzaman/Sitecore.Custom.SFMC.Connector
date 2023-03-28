// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.DateRangeConditionExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  [SupportedIds(new string[] {"{A938927D-B8BB-4411-9362-4D899AD0C407}"})]
  public class DateRangeConditionExpressionConverter : BaseConditionExpressionConverter
  {
    public const string FieldNameStartDate = "StartDate";
    public const string FieldNameEndDate = "EndDate";
    public const string FieldNameDateType = "DateType";
    public const string DateRangeConditionOperatorsGreaterOrThanEqualTo = "DateRangeConditionOperatorsGreaterOrThanEqualTo";
    public const string DateRangeConditionOperatorsLessThanOrEqualTo = "DateRangeConditionOperatorsLessThanOrEqualTo";

    public DateRangeConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source)
    {
      DateTime dateTime = this.GetDateTimeValue(source, "StartDate");
      if (dateTime == DateTime.MinValue)
        dateTime = SalesforceDateTime.MinValue;
      DateTime dateTimeValue = this.GetDateTimeValue(source, "EndDate");
      if (dateTime == DateTime.MinValue)
        dateTime = SalesforceDateTime.MaxValue;
      string shortStartDate = dateTime.ToString(SalesforceDateTypeFormat.DateTimeFormat);
      string shortEndDate = dateTimeValue.ToString(SalesforceDateTypeFormat.DateTimeFormat);
      if (this.GetDateTypeOperator(source) == "Date")
      {
        shortStartDate = dateTime.ToLocalTime().ToString(SalesforceDateTypeFormat.DateFormat);
        shortEndDate = dateTimeValue.ToLocalTime().ToString(SalesforceDateTypeFormat.DateFormat);
      }
      return (Func<IValueAccessor, object, object[]>) ((va, obj) => new object[2]
      {
        (object) shortStartDate,
        (object) shortEndDate
      });
    }

    protected override ConvertResult<ConditionExpressionDescriptor> ConvertSupportedItem(
      ItemModel source)
    {
      List<string> stringList = new List<string>();
      source.Add("DateRangeConditionOperatorsGreaterOrThanEqualTo", (object) Sitecore.DataExchange.ItemIDs.DateConditionOperatorsGreaterOrThanEqualTo);
      source.Add("DateRangeConditionOperatorsLessThanOrEqualTo", (object) Sitecore.DataExchange.ItemIDs.DateConditionOperatorsLessThanOrEqualTo);
      string fieldName = this.GetFieldName(source);
      stringList.Add(this.GetConditionOperator(source, "DateRangeConditionOperatorsGreaterOrThanEqualTo"));
      stringList.Add(this.GetConditionOperator(source, "DateRangeConditionOperatorsLessThanOrEqualTo"));
      Func<IValueAccessor, object, object[]> delegateForGetValues = this.GetDelegateForGetValues(source);
      return this.PositiveResult(new ConditionExpressionDescriptor()
      {
        FieldName = fieldName,
        OperatorsList = stringList,
        GetValues = delegateForGetValues
      });
    }

    protected virtual string GetDateTypeOperator(ItemModel source)
    {
      Guid guidValue = this.GetGuidValue(source, "DateType");
      return SalesforceDateType._salesforceDateType.ContainsKey(guidValue) ? SalesforceDateType._salesforceDateType[guidValue] : (string) null;
    }
  }
}
