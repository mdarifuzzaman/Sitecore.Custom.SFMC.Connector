// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.DataAccess.Writers.ExpandoObjectPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Writers;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.Writers
{
  internal class ExpandoObjectPropertyValueWriter : IndexerPropertyValueWriter
  {
    public ExpandoObjectPropertyValueWriter(params object[] indexes)
      : base(indexes)
    {
    }

    public override bool Write(object target, object value, DataAccessContext context)
    {
      if (!(target is IDictionary<string, object> dictionary))
        return false;
      foreach (object index in this.Indexes)
      {
        string key = index.ToString();
        if (!string.IsNullOrWhiteSpace(key))
        {
          if (dictionary.ContainsKey(key))
            dictionary[key] = value;
          else
            dictionary.Add(key, value);
        }
      }
      return true;
    }
  }
}
