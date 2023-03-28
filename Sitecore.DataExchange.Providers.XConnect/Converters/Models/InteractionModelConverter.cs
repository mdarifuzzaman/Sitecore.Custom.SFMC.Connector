// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.Models.InteractionModelConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.Models
{
  public class InteractionModelConverter : IConverter<Interaction, InteractionModel>
  {
    public virtual ConvertResult<InteractionModel> Convert(
      Interaction source)
    {
      if (source == null)
        return ConvertResult<InteractionModel>.PositiveResult((InteractionModel) null);
      InteractionModel convertedValue = new InteractionModel(source.Contact, source.Initiator, source.ChannelId, source.UserAgent)
      {
        DeviceProfile = source.DeviceProfile
      };
      if (source.Id.HasValue)
        convertedValue.Id = source.Id.Value;
      convertedValue.ContactReference = source.Contact;
      convertedValue.Initiator = source.Initiator;
      convertedValue.Facets = (IDictionary<string, Facet>) source.Facets.ToDictionary<KeyValuePair<string, Facet>, string, Facet>((Func<KeyValuePair<string, Facet>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, Facet>, Facet>) (kvp => kvp.Value));
      if (source.Events != null)
      {
        foreach (Event @event in (Collection<Event>) source.Events)
          convertedValue.Events.Add(@event);
      }
      return ConvertResult<InteractionModel>.PositiveResult(convertedValue);
    }
  }
}
