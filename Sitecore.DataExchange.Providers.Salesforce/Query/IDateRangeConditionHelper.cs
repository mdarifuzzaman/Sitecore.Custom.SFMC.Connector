// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.IDateRangeConditionHelper
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Plugins;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  public interface IDateRangeConditionHelper
  {
    DateRangeConditionDescriptor GenerateExpression(
      string fieldName,
      DateRangeSettings settings);

    DateRangeConditionDescriptor GenerateExpression(
      string fieldName,
      DateTime baseDateTime,
      int offsetValue,
      DateOffsetOperator offsetOperator,
      UnitOfTime unitOfTime);
  }
}
