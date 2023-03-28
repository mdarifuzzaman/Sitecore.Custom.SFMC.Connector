// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.ContactIdentifierValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class ContactIdentifierValueWriter : IValueWriter
  {
    public ContactIdentifierValueWriter(
      string identifierSource,
      ContactIdentifierType identifierType,
      bool rewrite = false)
    {
      this.IdentifierSource = identifierSource;
      this.IdentifierType = identifierType;
      this.Rewrite = rewrite;
    }

    public string IdentifierSource { get; protected set; }

    public ContactIdentifierType IdentifierType { get; protected set; }

    public bool Rewrite { get; protected set; }

    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      try
      {
        string identifier1 = value.ToString();
        if (!this.CanWrite(target, value, context) || string.IsNullOrEmpty(identifier1) || !(target is ContactModel contactModel))
          return false;
        if (!contactModel.HasIdentifier((Func<ContactIdentifier, bool>) (i => i.Source == this.IdentifierSource)))
        {
          contactModel.SetIdentifier(new ContactIdentifier(this.IdentifierSource, identifier1, this.IdentifierType));
          return true;
        }
        if (this.Rewrite)
        {
          ContactIdentifier identifier2 = contactModel.GetIdentifier((Func<ContactIdentifier, bool>) (i => i.Source == this.IdentifierSource));
          if (contactModel.ContactIdentifiers.Remove(identifier2))
          {
            contactModel.SetIdentifier(new ContactIdentifier(this.IdentifierSource, identifier1, this.IdentifierType));
            return true;
          }
        }
        return false;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    protected virtual bool CanWrite(object target, object value, DataAccessContext context) => target is ContactModel && !string.IsNullOrWhiteSpace(this.IdentifierSource);
  }
}
