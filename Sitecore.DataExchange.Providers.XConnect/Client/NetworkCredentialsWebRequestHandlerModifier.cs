// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Client.NetworkCredentialsWebRequestHandlerModifier
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.Xdb.Common.Web;
using System.Net;
using System.Net.Http;

namespace Sitecore.DataExchange.Providers.XConnect.Client
{
  public class NetworkCredentialsWebRequestHandlerModifier : IHttpClientHandlerModifier
  {
    public NetworkCredentialsWebRequestHandlerModifier(string userName, string password)
    {
      this.UserName = userName;
      this.Password = password;
    }

    protected string UserName { get; private set; }

    protected string Password { get; private set; }

    public void Process(HttpClientHandler handler)
    {
      handler.UseDefaultCredentials = false;
      handler.Credentials = (ICredentials) new NetworkCredential(this.UserName, this.Password);
    }
  }
}
