// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.ConditionExpressionDescriptor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  public class ConditionExpressionDescriptor : IExpressionDescriptor<string>
  {
    public IValueAccessor ValueAccessor { get; set; }

    public string FieldName { get; set; }

    public string ObjectType { get; set; }

    public string Operator { get; set; }

    public List<string> OperatorsList { get; set; }

    public Func<IValueAccessor, object, object[]> GetValues { get; set; }

    public virtual string ToExpression(object obj)
    {
      object[] objArray = this.GetValues(this.ValueAccessor, obj);
      if (objArray == null || objArray.Length == 0)
        return (string) null;
      List<string> stringList = new List<string>();
      int index = 0;
      foreach (object obj1 in objArray)
      {
        if (this.OperatorsList != null && this.OperatorsList.Any<string>())
        {
          stringList.Add(string.Format("{0} {1} {2}", (object) this.FieldName, (object) this.OperatorsList[index], obj1));
          ++index;
        }
        else
          stringList.Add(string.Format("{0} {1} {2}", (object) this.FieldName, (object) this.Operator, obj1));
      }
      string empty = string.Empty;
      string expression = this.OperatorsList == null || !this.OperatorsList.Any<string>() ? string.Join(" OR ", stringList.ToArray()) : string.Join(" AND ", stringList.ToArray());
      if (stringList.Count > 1)
        expression = "(" + expression + ")";
      return expression;
    }
  }
}
