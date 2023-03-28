// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ObjectReaderContext
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  public class ObjectReaderContext
  {
    public ObjectReaderContext(string objectType) => this.ObjectType = objectType;

    public string ObjectType { get; private set; }

    public IEnumerable<string> FieldNames { get; set; }

    public int MaxCount { get; set; }

    public string FilterExpression { get; set; }
  }
}
