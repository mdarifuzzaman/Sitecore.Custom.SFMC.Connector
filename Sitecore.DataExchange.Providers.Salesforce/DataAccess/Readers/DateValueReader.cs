// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers.DateValueReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers
{
  public class DateValueReader : IValueReader
  {
    public ReadResult Read(object source, DataAccessContext context)
    {
      object obj = (object) null;
      if (source != null)
        obj = (object) System.Convert.ToDateTime(source as string);
      return new ReadResult(DateTime.Now)
      {
        ReadValue = obj,
        WasValueRead = true
      };
    }
  }
}
