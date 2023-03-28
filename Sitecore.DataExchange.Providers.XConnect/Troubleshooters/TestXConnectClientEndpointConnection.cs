// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Troubleshooters.TestXConnectClientEndpointConnection
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Troubleshooters;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Troubleshooters
{
  public class TestXConnectClientEndpointConnection : BaseEndpointTroubleshooter
  {
    protected override ITroubleshooterResult Troubleshoot(
      Endpoint endpoint,
      TroubleshooterContext context)
    {
      try
      {
        XConnectClientEndpointSettings plugin = endpoint.GetPlugin<XConnectClientEndpointSettings>();
        if (plugin == null || plugin.ClientSettings == null)
          return (ITroubleshooterResult) TroubleshooterResult.FailResult("Connection model is null.");
        if (this.GetXConnectClient(plugin) == null)
          return (ITroubleshooterResult) TroubleshooterResult.FailResult("Cannot establish connection. Client is null.");
      }
      catch (Exception ex)
      {
        this.Logger.Error(ex.StackTrace);
        return (ITroubleshooterResult) TroubleshooterResult.FailResult("Exception was thrown. Read more in log file. " + ex.Message, ex);
      }
      return (ITroubleshooterResult) TroubleshooterResult.SuccessResult("Connection was successfully established.");
    }

    protected virtual IXdbContext GetXConnectClient(
      XConnectClientEndpointSettings settings)
    {
      return settings == null || settings.ClientHelper == null || settings.ClientSettings == null ? (IXdbContext) null : settings.ClientHelper.ToXConnectClient(settings.ClientSettings);
    }
  }
}
