// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.Models.InteractionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.Models
{
  public class InteractionConverter : IConverter<InteractionModel, Interaction>
  {
    public virtual ConvertResult<Interaction> Convert(
      InteractionModel source)
    {
      if (source == null)
        return ConvertResult<Interaction>.PositiveResult((Interaction) null);
      Interaction convertedValue = new Interaction(source.ContactReference, source.Initiator, source.ChannelId, source.UserAgent)
      {
        DeviceProfile = source.DeviceProfile
      };
      if (source.CampaignId != Guid.Empty)
        convertedValue.CampaignId = new Guid?(source.CampaignId);
      if (source.VenueId != Guid.Empty)
        convertedValue.VenueId = new Guid?(source.VenueId);
      foreach (Event @event in source.Events)
        convertedValue.Events.Add(@event);
      return ConvertResult<Interaction>.PositiveResult(convertedValue);
    }
  }
}
