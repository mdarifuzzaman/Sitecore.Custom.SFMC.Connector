// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Endpoints.SalesforceClientEndpointConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Data.Common;

namespace Sitecore.DataExchange.Providers.Salesforce.Endpoints
{
  [SupportedIds(new string[] {"{ACBF37D5-B08F-4E4C-9020-A9382B33040E}"})]
  public class SalesforceClientEndpointConverter : BaseConnectionStringEndpointConverter
  {
    private static bool _isRepeatInitialized;

    public SalesforceClientEndpointConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, Endpoint endpoint)
    {
      this.InitializeConnectorsSfcrmInstall();
      string connectionString = this.GetConnectionString(source);
      if (string.IsNullOrEmpty(connectionString))
        return;
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder()
      {
        ConnectionString = connectionString
      };
      if (string.IsNullOrWhiteSpace(connectionStringBuilder.ConnectionString))
        return;
      string username = (string) connectionStringBuilder["user id"];
      string password = (string) connectionStringBuilder["password"];
      string clientId = (string) connectionStringBuilder["client id"];
      string clientSecret = (string) connectionStringBuilder["secret key"];
      string securityToken = (string) connectionStringBuilder["security token"];
      bool result = false;
      if (connectionStringBuilder.ContainsKey("sandbox"))
        bool.TryParse((string) connectionStringBuilder["sandbox"], out result);
      AuthenticationClientSettings newPlugin = new AuthenticationClientSettings(clientId, clientSecret, username, password, securityToken)
      {
        IsSandbox = result
      };
      endpoint.AddPlugin<AuthenticationClientSettings>(newPlugin);
    }

    private void InitializeConnectorsSfcrmInstall()
    {
      if (SalesforceClientEndpointConverter._isRepeatInitialized)
        return;
      SalesforceClientEndpointConverter._isRepeatInitialized = true;
      Sitecore.DataExchange.Providers.Salesforce.Telemetry.TrackConnectorsSfCrmInstall();
    }
  }
}
