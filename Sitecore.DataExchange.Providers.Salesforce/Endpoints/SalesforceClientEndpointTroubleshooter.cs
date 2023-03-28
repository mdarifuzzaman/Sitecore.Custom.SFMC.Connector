// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Endpoints.SalesforceClientEndpointTroubleshooter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Troubleshooters;
using System;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.Endpoints
{
  public class SalesforceClientEndpointTroubleshooter : BaseEndpointTroubleshooter
  {
    protected override ITroubleshooterResult Troubleshoot(
      Endpoint endpoint,
      TroubleshooterContext context)
    {
      if (endpoint == null)
        return (ITroubleshooterResult) TroubleshooterResult.FailResult("Endpoint is null.");
      AuthenticationClientSettings settings = endpoint.GetPlugin<AuthenticationClientSettings>();
      if (settings == null)
        return (ITroubleshooterResult) TroubleshooterResult.FailResult("Endpoint settings plugin is null.");
      if (settings.AuthenticationClient == null)
        return (ITroubleshooterResult) TroubleshooterResult.FailResult("Authentication client is null on the endpoint settings plugin. This may be because no connection string is specified, or because the specified connection string does not exist.");
      try
      {
        Task.Run((Func<Task>) (async () => await settings.AuthenticationClient.TokenRefreshAsync(settings.AuthenticationClient.Id, settings.AuthenticationClient.RefreshToken, "")));
      }
      catch (Exception ex)
      {
        return (ITroubleshooterResult) TroubleshooterResult.FailResult("Exception during connection. Read more in log file. " + ex.Message, ex);
      }
      return (ITroubleshooterResult) TroubleshooterResult.SuccessResult("Connection was successfully established.");
    }
  }
}
