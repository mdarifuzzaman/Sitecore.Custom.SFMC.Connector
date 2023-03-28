// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Extensions.ContactModelExtensions
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Extensions
{
  public static class ContactModelExtensions
  {
    public static void SetIdentifier(this ContactModel contactModel, ContactIdentifier identifier) => contactModel.ContactIdentifiers.Add(identifier);

    public static bool HasIdentifier(
      this ContactModel contactModel,
      Func<ContactIdentifier, bool> expression)
    {
      return contactModel.ContactIdentifiers.Any<ContactIdentifier>(expression);
    }

    public static ContactIdentifier GetIdentifier(
      this ContactModel contactModel,
      Func<ContactIdentifier, bool> expression)
    {
      return contactModel.ContactIdentifiers.FirstOrDefault<ContactIdentifier>(expression);
    }

    public static IEnumerable<ContactIdentifier> GetIdentifiers(
      this ContactModel contactModel,
      Func<ContactIdentifier, bool> expression)
    {
      return contactModel.ContactIdentifiers.Where<ContactIdentifier>(expression);
    }
  }
}
