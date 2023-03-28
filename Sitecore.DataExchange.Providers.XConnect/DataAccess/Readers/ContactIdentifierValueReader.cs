// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.ContactIdentifierValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
  public class ContactIdentifierValueReader : IValueReader
  {
    public ContactIdentifierValueReader(
      string identifierSource,
      ContactIdentifierType identifierType)
    {
      this.IdentifierSource = identifierSource;
      this.IdentifierType = identifierType;
    }

    public string IdentifierSource { get; private set; }

    public ContactIdentifierType IdentifierType { get; private set; }

    public virtual ReadResult Read(object source, DataAccessContext context)
    {
      if (!this.CanRead(source, context))
        return ReadResult.NegativeResult(DateTime.Now);
      ContactModel contactModel = (ContactModel) source;
      ContactIdentifier readValue = contactModel != null ? contactModel.GetIdentifier((Func<ContactIdentifier, bool>) (ci => ci.Source == this.IdentifierSource && ci.IdentifierType == this.IdentifierType)) : (ContactIdentifier) null;
      return readValue == null ? ReadResult.NegativeResult(DateTime.Now) : ReadResult.PositiveResult((object) readValue, DateTime.Now);
    }

    protected virtual bool CanRead(object source, DataAccessContext context) => source is ContactModel contactModel && !string.IsNullOrWhiteSpace(this.IdentifierSource) && contactModel.HasIdentifier((Func<ContactIdentifier, bool>) (ci => ci.Source == this.IdentifierSource));
  }
}
