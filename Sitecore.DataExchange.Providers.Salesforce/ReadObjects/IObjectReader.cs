// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.IObjectReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Force;
using System.Collections.Generic;
using System.Dynamic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  public interface IObjectReader
  {
    ExpandoObject ReadObject(ForceClient client, ObjectReaderContext context);

    IEnumerable<ExpandoObject> ReadObjects(
      ForceClient client,
      ObjectReaderContext context);
  }
}
