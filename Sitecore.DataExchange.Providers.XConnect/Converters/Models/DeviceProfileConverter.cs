// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.Models.DeviceProfileConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.Models
{
  public class DeviceProfileConverter : IConverter<DeviceProfileModel, DeviceProfile>
  {
    public virtual ConvertResult<DeviceProfile> Convert(
      DeviceProfileModel source)
    {
      if (source == null)
        return ConvertResult<DeviceProfile>.PositiveResult((DeviceProfile) null);
      if (!(source.Entity is DeviceProfile convertedValue))
        convertedValue = new DeviceProfile(source.Id)
        {
          LastKnownContact = source.LastKnownContact
        };
      return ConvertResult<DeviceProfile>.PositiveResult(convertedValue);
    }
  }
}
