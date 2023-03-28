// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ObjectReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common.Models.Json;
using Salesforce.Force;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  public class ObjectReader : IObjectReader
  {
    public virtual IEnumerable<ExpandoObject> ReadObjects(
      ForceClient client,
      ObjectReaderContext context)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      string soql = context != null ? this.GetSoqlStatement(context) : throw new ArgumentNullException(nameof (context));
      for (QueryResult<ExpandoObject> result = Task.Run<QueryResult<ExpandoObject>>((Func<Task<QueryResult<ExpandoObject>>>) (() => client.QueryAsync<ExpandoObject>(soql))).Result; result != null; result = Task.Run<QueryResult<ExpandoObject>>((Func<Task<QueryResult<ExpandoObject>>>) (() => client.QueryContinuationAsync<ExpandoObject>(result.NextRecordsUrl))).Result)
      {
        foreach (ExpandoObject record in result.Records)
          yield return record;
        if (result.Done || string.IsNullOrWhiteSpace(result.NextRecordsUrl))
          break;
      }
    }

    public virtual ExpandoObject ReadObject(
      ForceClient client,
      ObjectReaderContext context)
    {
      IEnumerable<ExpandoObject> source = this.ReadObjects(client, context);
      return source.Any<ExpandoObject>() ? source.First<ExpandoObject>() : (ExpandoObject) null;
    }

    protected string GetSoqlFieldList(ObjectReaderContext context) => context.FieldNames.Any<string>() ? string.Join(", ", context.FieldNames.ToArray<string>()) : throw new InvalidOperationException("At least one field name must be specified.");

    protected string GetSoqlFromExpression(ObjectReaderContext context) => !string.IsNullOrWhiteSpace(context.ObjectType) ? context.ObjectType : throw new InvalidOperationException("The object type must be specified.");

    protected string GetSoqlWhereExpression(ObjectReaderContext context) => context.FilterExpression;

    protected string GetSoqlGroupByExpression(ObjectReaderContext context) => (string) null;

    protected string GetSoqlOrderByExpression(ObjectReaderContext context) => (string) null;

    protected int GetSoqlLimitValue(ObjectReaderContext context) => context.MaxCount;

    protected int GetSoqlOffsetValue(ObjectReaderContext context) => 0;

    protected string GetSoqlForExpression(ObjectReaderContext context) => (string) null;

    protected virtual string GetSoqlStatement(ObjectReaderContext context)
    {
      StringBuilder stringBuilder = new StringBuilder("SELECT " + this.GetSoqlFieldList(context) + " FROM " + this.GetSoqlFromExpression(context));
      string soqlWhereExpression = this.GetSoqlWhereExpression(context);
      if (!string.IsNullOrWhiteSpace(soqlWhereExpression))
        stringBuilder.Append(" WHERE " + soqlWhereExpression);
      string groupByExpression = this.GetSoqlGroupByExpression(context);
      if (!string.IsNullOrWhiteSpace(groupByExpression))
        stringBuilder.Append(" GROUP BY " + groupByExpression);
      string orderByExpression = this.GetSoqlOrderByExpression(context);
      if (!string.IsNullOrWhiteSpace(orderByExpression))
        stringBuilder.Append(" ORDER BY " + orderByExpression);
      int soqlLimitValue = this.GetSoqlLimitValue(context);
      if (soqlLimitValue > 0)
        stringBuilder.Append(string.Format(" LIMIT {0}", (object) soqlLimitValue));
      int soqlOffsetValue = this.GetSoqlOffsetValue(context);
      if (soqlOffsetValue > 0)
        stringBuilder.Append(string.Format(" OFFSET {0}", (object) soqlOffsetValue));
      string soqlForExpression = this.GetSoqlForExpression(context);
      if (!string.IsNullOrWhiteSpace(soqlForExpression))
        stringBuilder.Append(" FOR " + soqlForExpression);
      return stringBuilder.ToString();
    }
  }
}
