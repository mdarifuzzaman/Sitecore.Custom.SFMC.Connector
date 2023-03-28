// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers.LookupFieldValuesReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers
{
  public class LookupFieldValuesReader : IValueReader
  {
    public LookupFieldValuesReader(string propertyName) => this.PropertyName = propertyName;

    public string PropertyName { get; private set; }

    public virtual ReadResult Read(object source, DataAccessContext context)
    {
      if (!this.CanRead(source, context))
        return ReadResult.NegativeResult(DateTime.Now);
      IDictionary<string, object> dictionary1 = source as IDictionary<string, object>;
      if (!dictionary1.ContainsKey("records") || !(dictionary1["records"] is IEnumerable<object> objects))
        return ReadResult.NegativeResult(DateTime.Now);
      List<string> readValue = new List<string>();
      foreach (IDictionary<string, object> dictionary2 in objects)
      {
        foreach (string key in (IEnumerable<string>) dictionary2.Keys)
        {
          if (key.Equals(this.PropertyName, StringComparison.OrdinalIgnoreCase))
            readValue.Add(dictionary2[key].ToString());
        }
      }
      return ReadResult.PositiveResult((object) readValue, DateTime.Now);
    }

    protected virtual bool CanRead(object source, DataAccessContext context) => source is IDictionary<string, object>;
  }
}
