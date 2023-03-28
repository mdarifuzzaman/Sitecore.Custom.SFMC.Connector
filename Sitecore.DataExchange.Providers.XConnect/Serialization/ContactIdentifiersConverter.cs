// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Serialization.ContactIdentifiersConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Serialization
{
  public class ContactIdentifiersConverter : JsonConverter
  {
    public override bool CanRead { get; }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, (object) ((IEnumerable<ContactIdentifier>) value).Where<ContactIdentifier>((Func<ContactIdentifier, bool>) (c => c.Source != "Alias")).ToArray<ContactIdentifier>());

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType) => objectType == typeof (ReadOnlyCollection<ContactIdentifier>);
  }
}
