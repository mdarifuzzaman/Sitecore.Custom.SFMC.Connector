// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.Models.ContactModelConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.Models
{
  public class ContactModelConverter : IConverter<Contact, ContactModel>
  {
    public virtual ConvertResult<ContactModel> Convert(Contact source)
    {
      if (source == null)
        return ConvertResult<ContactModel>.PositiveResult((ContactModel) null);
      ContactModel contactModel = new ContactModel();
      contactModel.Entity = (Entity) source;
      ContactModel convertedValue = contactModel;
      if (source.Id.HasValue)
        convertedValue.Id = source.Id.Value;
      convertedValue.Facets = (IDictionary<string, Facet>) source.Facets.ToDictionary<KeyValuePair<string, Facet>, string, Facet>((Func<KeyValuePair<string, Facet>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, Facet>, Facet>) (kvp => kvp.Value));
      convertedValue.ContactIdentifiers = source.Identifiers.ToList<ContactIdentifier>();
      convertedValue.Interactions = source.Interactions;
      return ConvertResult<ContactModel>.PositiveResult(convertedValue);
    }
  }
}
