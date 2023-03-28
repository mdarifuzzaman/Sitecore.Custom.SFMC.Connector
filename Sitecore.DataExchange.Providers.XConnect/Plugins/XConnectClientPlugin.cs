// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Plugins.XConnectClientPlugin
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Plugins
{
  public class XConnectClientPlugin : BaseXpfClientPlugin<IXdbContext>
  {
    public XConnectClientPlugin(XConnectClientSettings clientSettings, IXConnectClientHelper helper)
    {
      this.ClientSettings = clientSettings;
      this.ClientHelper = helper;
    }

    public XConnectClientSettings ClientSettings { get; private set; }

    public IXConnectClientHelper ClientHelper { get; private set; }

    public int MinimumBatchSize { get; set; }

    protected override IXdbContext CreateNewClient(int threadId) => this.ClientHelper.ToXConnectClient(this.ClientSettings);
  }
}
