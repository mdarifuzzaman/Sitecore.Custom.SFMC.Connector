// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.ContactIdentifierByConstructorValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class ContactIdentifierByConstructorValueWriter : IValueWriter
  {
    public readonly IMappingSet MappingSet;

    public ContactIdentifierByConstructorValueWriter(IMappingSet mappingSet) => this.MappingSet = mappingSet;

    public bool Write(object target, object value, DataAccessContext context)
    {
      if (!this.CanWrite(target, value, context))
        return false;
      ContactModel contactModel = target as ContactModel;
      Dictionary<int, object> dictionary = new Dictionary<int, object>();
      if (!this.MappingSet.Run(new MappingContext()
      {
        Source = value,
        Target = (object) dictionary
      }))
        return false;
      if (!dictionary.ContainsKey(1) && string.IsNullOrWhiteSpace(dictionary[1].ToString()))
        return false;
      string source = dictionary[1].ToString();
      if (!dictionary.ContainsKey(2))
        return false;
      string identifier = dictionary[2].ToString();
      if (contactModel.ContactIdentifiers.Any<ContactIdentifier>((Func<ContactIdentifier, bool>) (i => i.Source == source && i.Identifier == identifier)))
        return true;
      if (!dictionary.ContainsKey(3) || dictionary[3] == null)
        dictionary[3] = (object) ContactIdentifierType.Known;
      ContactIdentifierType identifierType = ContactIdentifierType.Anonymous;
      if (Enum.IsDefined(typeof (ContactIdentifierType), dictionary[3]))
        identifierType = (ContactIdentifierType) dictionary[3];
      contactModel?.ContactIdentifiers.Add(new ContactIdentifier(source, identifier, identifierType));
      return true;
    }

    protected virtual bool CanWrite(object target, object value, DataAccessContext context) => target is ContactModel;
  }
}
