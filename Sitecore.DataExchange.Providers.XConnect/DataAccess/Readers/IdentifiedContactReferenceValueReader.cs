// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.IdentifiedContactReferenceValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
  public class IdentifiedContactReferenceValueReader : IValueReader
  {
    public IdentifiedContactReferenceValueReader(string identifierSource) => this.IdentifierSource = identifierSource;

    public string IdentifierSource { get; private set; }

    public virtual ReadResult Read(object source, DataAccessContext context)
    {
      Guid? guid = this.ConvertToGuid(source);
      DateTime now = DateTime.Now;
      return !guid.HasValue || !guid.HasValue || guid.Value == Guid.Empty ? ReadResult.NegativeResult(now) : ReadResult.PositiveResult((object) new IdentifiedContactReference(this.IdentifierSource, guid.Value.ToString()), now);
    }

    protected virtual Guid? ConvertToGuid(object source)
    {
      if (source == null)
        return new Guid?();
      if (source is Guid guid)
        return new Guid?(guid);
      Guid result = Guid.Empty;
      return Guid.TryParse(source.ToString(), out result) ? new Guid?(result) : new Guid?();
    }
  }
}
