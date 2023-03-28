// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Plugins.BaseXpfClientPlugin`1
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sitecore.DataExchange.Providers.XConnect.Plugins
{
  public abstract class BaseXpfClientPlugin<TClientType> : IPlugin
  {
    private IDictionary<int, TClientType> _clients = (IDictionary<int, TClientType>) new Dictionary<int, TClientType>();
    private object _lock = new object();

    public virtual TClientType GetClient(int threadId)
    {
      lock (this._lock)
      {
        if (this._clients.ContainsKey(threadId))
          return this._clients[threadId];
        TClientType newClient = this.CreateNewClient(threadId);
        if ((object) newClient != null)
          this._clients[threadId] = newClient;
        return newClient;
      }
    }

    public virtual IDictionary<int, TClientType> GetClients() => (IDictionary<int, TClientType>) new ReadOnlyDictionary<int, TClientType>(this._clients);

    protected abstract TClientType CreateNewClient(int threadId);
  }
}
