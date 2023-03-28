// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Endpoints.AuthenticationClientSettings
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common;
using System;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.Endpoints
{
  public class AuthenticationClientSettings : IPlugin
  {
    private byte[] _clientId;
    private byte[] _clientSecret;
    private byte[] _username;
    private byte[] _password;
    private byte[] _securityToken;
    private bool _isTokenRequestEndpointUrlExplicitlySet;
    private string _tokenRequestEndpointUrl;
    private AuthenticationClient _client;

    public AuthenticationClientSettings(
      string clientId,
      string clientSecret,
      string username,
      string password,
      string securityToken)
    {
      this._clientId = ProtectedDataHelper.ProtectData(clientId);
      this._clientSecret = ProtectedDataHelper.ProtectData(clientSecret);
      this._username = ProtectedDataHelper.ProtectData(username);
      this._password = ProtectedDataHelper.ProtectData(password);
      this._securityToken = ProtectedDataHelper.ProtectData(securityToken);
    }

    public bool IsSandbox { get; set; }

    public string TokenRequestEndpointUrl
    {
      get => this._isTokenRequestEndpointUrlExplicitlySet ? this._tokenRequestEndpointUrl : this.GetDefaultTokenRequestEndpointUrl();
      set
      {
        this._isTokenRequestEndpointUrlExplicitlySet = true;
        this._tokenRequestEndpointUrl = value;
      }
    }

    public AuthenticationClient AuthenticationClient
    {
      get
      {
        if (this._client == null)
          this._client = new AuthenticationClient();
        Task.Run((Func<Task>) (async () => await this._client.UsernamePasswordAsync(ProtectedDataHelper.UnprotectData(this._clientId), ProtectedDataHelper.UnprotectData(this._clientSecret), ProtectedDataHelper.UnprotectData(this._username), ProtectedDataHelper.UnprotectData(this._password) + ProtectedDataHelper.UnprotectData(this._securityToken), this.TokenRequestEndpointUrl).ConfigureAwait(false))).Wait();
        return this._client;
      }
    }

    protected virtual string GetDefaultTokenRequestEndpointUrl() => this.IsSandbox ? "https://test.salesforce.com/services/oauth2/token" : "https://login.salesforce.com/services/oauth2/token";
  }
}
