// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.DateRangeConditionHelper
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Plugins;
using System;
using System.Text;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  public class DateRangeConditionHelper : IDateRangeConditionHelper
  {
    private static DateRangeSettingsHelper _helper = new DateRangeSettingsHelper();

    public DateRangeConditionHelper() => this.Helper = DateRangeConditionHelper._helper;

    protected DateRangeSettingsHelper Helper { get; set; }

    public virtual DateRangeConditionDescriptor GenerateExpression(
      string fieldName,
      DateRangeSettings settings)
    {
      DateRangeConditionDescriptor expression = new DateRangeConditionDescriptor();
      StringBuilder stringBuilder = new StringBuilder();
      if (settings.LowerBound > SalesforceDateTime.MinValue)
      {
        expression.Min = new DateTime?(settings.LowerBound);
        stringBuilder.Append(fieldName + " >= " + settings.LowerBound.ToString("yyyy-MM-ddThh:mm:ssZ"));
      }
      if (settings.UpperBound < SalesforceDateTime.MaxValue)
      {
        expression.Max = new DateTime?(settings.UpperBound);
        if (stringBuilder.Length > 0)
          stringBuilder.Append(" AND ");
        stringBuilder.Append(fieldName + " <= " + settings.UpperBound.ToString("yyyy-MM-ddThh:mm:ssZ"));
      }
      if (stringBuilder.Length > 0)
        expression.Expression = stringBuilder.ToString();
      return expression;
    }

    public virtual DateRangeConditionDescriptor GenerateExpression(
      string fieldName,
      DateTime baseDateTime,
      int offsetValue,
      DateOffsetOperator offsetOperator,
      UnitOfTime unitOfTime)
    {
      DateRangeSettings settings = this.Helper.Create(baseDateTime, offsetOperator, offsetValue, unitOfTime);
      return this.GenerateExpression(fieldName, settings);
    }
  }
}
