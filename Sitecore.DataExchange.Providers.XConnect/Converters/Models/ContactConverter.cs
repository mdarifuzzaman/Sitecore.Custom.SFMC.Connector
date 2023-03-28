// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.Models.ContactConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.Models
{
  public class ContactConverter : IConverter<ContactModel, Contact>
  {
    public virtual ConvertResult<Contact> Convert(ContactModel source)
    {
      if (source == null)
        return ConvertResult<Contact>.PositiveResult((Contact) null);
      if (!(source.Entity is Contact convertedValue))
        convertedValue = new Contact(source.ContactIdentifiers.ToArray());
      return ConvertResult<Contact>.PositiveResult(convertedValue);
    }
  }
}
