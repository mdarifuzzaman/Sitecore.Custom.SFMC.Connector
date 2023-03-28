// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.Models.DeviceProfileModelConverter
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
  public class DeviceProfileModelConverter : IConverter<DeviceProfile, DeviceProfileModel>
  {
    public virtual ConvertResult<DeviceProfileModel> Convert(
      DeviceProfile source)
    {
      if (source == null)
        return ConvertResult<DeviceProfileModel>.PositiveResult((DeviceProfileModel) null);
      DeviceProfileModel deviceProfileModel = new DeviceProfileModel();
      deviceProfileModel.Entity = (Entity) source;
      DeviceProfileModel convertedValue = deviceProfileModel;
      if (source.Id.HasValue)
        convertedValue.Id = source.Id.Value;
      convertedValue.Facets = (IDictionary<string, Facet>) source.Facets.ToDictionary<KeyValuePair<string, Facet>, string, Facet>((Func<KeyValuePair<string, Facet>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, Facet>, Facet>) (kvp => kvp.Value));
      convertedValue.LastKnownContact = source.LastKnownContact;
      return ConvertResult<DeviceProfileModel>.PositiveResult(convertedValue);
    }
  }
}
