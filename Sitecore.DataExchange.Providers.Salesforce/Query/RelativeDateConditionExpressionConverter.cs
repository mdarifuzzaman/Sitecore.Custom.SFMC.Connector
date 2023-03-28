// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.RelativeDateConditionExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  [SupportedIds(new string[] {"{B8E6F2D9-20C3-4F67-BDC2-39E3A3676059}"})]
  public class RelativeDateConditionExpressionConverter : BaseConditionExpressionConverter
  {
    public const string FieldNameOffset = "Offset";
    public const string FieldNameOperator = "Operator";
    public const string FieldNameUnitOfTime = "UnitOfTime";
    public const string FieldNameDateType = "DateType";
    public const string DateRangeConditionOperatorsGreaterOrThanEqualTo = "DateRangeConditionOperatorsGreaterOrThanEqualTo";
    public const string DateRangeConditionOperatorsLessThanOrEqualTo = "DateRangeConditionOperatorsLessThanOrEqualTo";
    private static readonly DateRangeSettingsHelper _helper = new DateRangeSettingsHelper();

    public RelativeDateConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
      this.DateRangeSettingsHelper = RelativeDateConditionExpressionConverter._helper;
    }

    public Func<DateTime> GetNow { get; set; }

    protected DateRangeSettingsHelper DateRangeSettingsHelper { get; set; }

    protected override Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source)
    {
      return (Func<IValueAccessor, object, object[]>) ((va, obj) => this.GetValues(DateTime.Now, source));
    }

    protected virtual object[] GetValues(DateTime baseDateTime, ItemModel source)
    {
      int intValue = this.GetIntValue(source, "Offset");
      UnitOfTime enumValue1 = this.GetEnumValue<UnitOfTime>(source, "UnitOfTime");
      if (enumValue1 == UnitOfTime.Unspecified)
        return new object[0];
      DateOffsetOperator enumValue2 = this.GetEnumValue<DateOffsetOperator>(source, "Operator");
      if (enumValue2 == DateOffsetOperator.Unspecified)
        return new object[0];
      DateRangeSettings dateRangeSettings = this.DateRangeSettingsHelper.Create(baseDateTime, enumValue2, intValue, enumValue1, this.GetNow);
      if (dateRangeSettings.LowerBound < SalesforceDateTime.MinValue)
        dateRangeSettings.LowerBound = SalesforceDateTime.MinValue;
      if (dateRangeSettings.UpperBound > SalesforceDateTime.MaxValue)
        dateRangeSettings.LowerBound = SalesforceDateTime.MaxValue;
      if (dateRangeSettings.LowerBound > dateRangeSettings.UpperBound)
      {
        DateTime upperBound = dateRangeSettings.UpperBound;
        dateRangeSettings.UpperBound = dateRangeSettings.LowerBound;
        dateRangeSettings.LowerBound = upperBound;
      }
      string str1 = dateRangeSettings.LowerBound.ToString(SalesforceDateTypeFormat.DateTimeFormat);
      string str2 = dateRangeSettings.UpperBound.ToString(SalesforceDateTypeFormat.DateTimeFormat);
      if (this.GetDateTypeOperator(source) == "Date")
      {
        str1 = dateRangeSettings.LowerBound.ToLocalTime().ToString(SalesforceDateTypeFormat.DateFormat);
        str2 = dateRangeSettings.UpperBound.ToLocalTime().ToString(SalesforceDateTypeFormat.DateFormat);
      }
      return new object[2]{ (object) str1, (object) str2 };
    }

    protected override ConvertResult<ConditionExpressionDescriptor> ConvertSupportedItem(
      ItemModel source)
    {
      string fieldName = this.GetFieldName(source);
      DateOffsetOperator enumValue = this.GetEnumValue<DateOffsetOperator>(source, "Operator");
      List<string> stringList = this.ConvertValueForOperatorsList(source, enumValue);
      Func<IValueAccessor, object, object[]> delegateForGetValues = this.GetDelegateForGetValues(source);
      return this.PositiveResult(new ConditionExpressionDescriptor()
      {
        FieldName = fieldName,
        OperatorsList = stringList,
        GetValues = delegateForGetValues
      });
    }

    protected virtual List<string> ConvertValueForOperatorsList(
      ItemModel source,
      DateOffsetOperator offsetOperator)
    {
      List<string> stringList = new List<string>();
      source.Add("DateRangeConditionOperatorsGreaterOrThanEqualTo", (object) Sitecore.DataExchange.ItemIDs.DateConditionOperatorsGreaterOrThanEqualTo);
      source.Add("DateRangeConditionOperatorsLessThanOrEqualTo", (object) Sitecore.DataExchange.ItemIDs.DateConditionOperatorsLessThanOrEqualTo);
      stringList.Add(this.GetConditionOperator(source, "DateRangeConditionOperatorsGreaterOrThanEqualTo"));
      stringList.Add(this.GetConditionOperator(source, "DateRangeConditionOperatorsLessThanOrEqualTo"));
      return stringList;
    }

    protected virtual string GetDateTypeOperator(ItemModel source)
    {
      Guid guidValue = this.GetGuidValue(source, "DateType");
      return SalesforceDateType._salesforceDateType.ContainsKey(guidValue) ? SalesforceDateType._salesforceDateType[guidValue] : (string) null;
    }
  }
}
