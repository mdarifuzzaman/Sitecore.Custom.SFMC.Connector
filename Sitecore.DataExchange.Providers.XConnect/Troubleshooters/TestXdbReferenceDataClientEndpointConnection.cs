// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Troubleshooters.TestXdbReferenceDataClientEndpointConnection
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.ReferenceData;
using Sitecore.DataExchange.Troubleshooters;
using Sitecore.Xdb.ReferenceData.Core;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Troubleshooters
{
  public class TestXdbReferenceDataClientEndpointConnection : BaseEndpointTroubleshooter
  {
    protected override ITroubleshooterResult Troubleshoot(
      Endpoint endpoint,
      TroubleshooterContext context)
    {
      try
      {
        ReferenceDataClientEndpointSettings plugin = endpoint.GetPlugin<ReferenceDataClientEndpointSettings>();
        if (plugin == null)
          return (ITroubleshooterResult) TroubleshooterResult.FailResult(string.Format("Endpoint plugin is missing. Plugin type: {0}", (object) "ReferenceDataClientEndpointSettings"));
        if (plugin.BaseAddress == (Uri) null)
          return (ITroubleshooterResult) TroubleshooterResult.FailResult("No base address for the xDB reference data service is specified.");
        IReferenceDataClient referenceDataClient = this.GetReferenceDataClient(plugin);
        if (referenceDataClient == null)
          return (ITroubleshooterResult) TroubleshooterResult.FailResult("Cannot get a reference to the object used to communicate with the service.");
        referenceDataClient.GetDefinitionType(Guid.NewGuid().ToString());
      }
      catch (Exception ex)
      {
        this.Logger.Error(ex.StackTrace);
        return (ITroubleshooterResult) TroubleshooterResult.FailResult("Exception was thrown. Read more in log file. " + ex.Message, ex);
      }
      return (ITroubleshooterResult) TroubleshooterResult.SuccessResult("Connection was successfully established.");
    }

    protected virtual IReferenceDataClient GetReferenceDataClient(
      ReferenceDataClientEndpointSettings settings)
    {
      return new ReferenceDataClientPlugin(settings.BaseAddress, typeof (string), typeof (string))
      {
        Modifiers = settings.Modifiers
      }.GetClient(0);
    }
  }
}
