// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Converters.ExpandoSObjectConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common.Models.Xml;
using System.Collections.Generic;
using System.Dynamic;

namespace Sitecore.DataExchange.Providers.Salesforce.Converters
{
  public static class ExpandoSObjectConverter
  {
    public static SObject Convert(ExpandoObject source)
    {
      if (source == null)
        return (SObject) null;
      IDictionary<string, object> dictionary = (IDictionary<string, object>) source;
      SObject sobject = new SObject();
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) dictionary)
        sobject.Add(keyValuePair.Key, keyValuePair.Value);
      return sobject;
    }
  }
}
