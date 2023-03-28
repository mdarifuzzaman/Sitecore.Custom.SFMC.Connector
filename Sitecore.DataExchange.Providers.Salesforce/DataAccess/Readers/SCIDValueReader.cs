// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers.SCIDValueReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers
{
  public class SCIDValueReader : IValueReader
  {
    public ReadResult Read(object source, DataAccessContext context)
    {
      if (source is Guid guid)
        return new ReadResult(DateTime.Now)
        {
          ReadValue = (object) guid,
          WasValueRead = true
        };
      return new ReadResult(DateTime.Now)
      {
        ReadValue = (object) this.GenerateGuidForSFId(source.ToString()),
        WasValueRead = true
      };
    }

    protected virtual bool CanRead(object source, DataAccessContext context) => source is Guid || source is string;

    private Guid GenerateGuidForSFId(string sourceItemSFId) => sourceItemSFId.Length > 18 ? new Guid(sourceItemSFId) : new Guid(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(sourceItemSFId)));
  }
}
