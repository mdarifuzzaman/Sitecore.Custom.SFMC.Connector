// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.FilterExpressionDescriptor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  public class FilterExpressionDescriptor : IExpressionDescriptor<string>
  {
    public FilterExpressionDescriptor() => this.ConditionExpressions = (ICollection<ConditionExpressionDescriptor>) new List<ConditionExpressionDescriptor>();

    public string LogicalOperator { get; set; }

    public ICollection<ConditionExpressionDescriptor> ConditionExpressions { get; private set; }

    public virtual string ToExpression(object obj)
    {
      List<string> stringList = new List<string>();
      foreach (ConditionExpressionDescriptor conditionExpression in (IEnumerable<ConditionExpressionDescriptor>) this.ConditionExpressions)
        stringList.Add(conditionExpression.ToExpression(obj));
      return stringList.Count == 0 ? (string) null : string.Join(" " + this.LogicalOperator + " ", stringList.ToArray());
    }
  }
}
