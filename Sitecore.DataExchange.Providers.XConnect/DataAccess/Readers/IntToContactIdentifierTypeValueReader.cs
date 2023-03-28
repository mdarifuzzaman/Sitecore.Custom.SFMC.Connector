// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.IntToContactIdentifierTypeValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
  public class IntToContactIdentifierTypeValueReader : IValueReader
  {
    public readonly IDictionary<int, string> TypeMapper;

    public IntToContactIdentifierTypeValueReader(IDictionary<int, string> typeMapper = null) => this.TypeMapper = typeMapper ?? (IDictionary<int, string>) new Dictionary<int, string>();

    public virtual ReadResult Read(object source, DataAccessContext context)
    {
      if (!this.CanRead(source, context))
        return ReadResult.NegativeResult(DateTime.Now);
      try
      {
        ContactIdentifierType contactIdentifierType;
        return !this.TryMap(Convert.ToInt32(source), out contactIdentifierType) ? ReadResult.NegativeResult(DateTime.Now) : ReadResult.PositiveResult((object) contactIdentifierType, DateTime.Now);
      }
      catch (Exception ex)
      {
        return ReadResult.NegativeResult(DateTime.Now);
      }
    }

    protected virtual bool CanRead(object source, DataAccessContext context) => source is int;

    protected virtual bool TryMap(int value, out ContactIdentifierType contactIdentifierType)
    {
      if (this.TypeMapper.ContainsKey(value))
        return Enum.TryParse<ContactIdentifierType>(this.TypeMapper[value], true, out contactIdentifierType);
      if (Enum.IsDefined(typeof (ContactIdentifierType), (object) value))
      {
        contactIdentifierType = (ContactIdentifierType) value;
        return true;
      }
      contactIdentifierType = ContactIdentifierType.Anonymous;
      return false;
    }
  }
}
