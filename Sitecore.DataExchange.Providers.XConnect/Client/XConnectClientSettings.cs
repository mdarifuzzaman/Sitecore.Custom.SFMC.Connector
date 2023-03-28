// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Client.XConnectClientSettings
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Client
{
  public class XConnectClientSettings
  {
    private IDictionary<XConnectServiceTypes, Uri> _uris;
    private IDictionary<XConnectServiceTypes, ICollection<IHttpClientModifier>> _clientModifiers;
    private IDictionary<XConnectServiceTypes, ICollection<IHttpClientHandlerModifier>> _requestHandlerModifiers;

    public XConnectClientSettings(string identifier, string hash)
    {
      if (string.IsNullOrWhiteSpace(identifier))
        throw new ArgumentNullException(nameof (identifier));
      if (string.IsNullOrWhiteSpace(hash))
        throw new ArgumentNullException(nameof (hash));
      this._uris = (IDictionary<XConnectServiceTypes, Uri>) new Dictionary<XConnectServiceTypes, Uri>();
      this._clientModifiers = (IDictionary<XConnectServiceTypes, ICollection<IHttpClientModifier>>) new Dictionary<XConnectServiceTypes, ICollection<IHttpClientModifier>>();
      this._requestHandlerModifiers = (IDictionary<XConnectServiceTypes, ICollection<IHttpClientHandlerModifier>>) new Dictionary<XConnectServiceTypes, ICollection<IHttpClientHandlerModifier>>();
      this.DefaultServiceType = XConnectServiceTypes.CollectionService;
      this.SettingsIdentifier = identifier;
      this.SettingsHash = hash;
    }

    public XdbModel CollectionModel { get; set; }

    public string SettingsIdentifier { get; private set; }

    public string SettingsHash { get; private set; }

    public bool UseServiceSettingsForAllServices { get; set; }

    public XConnectServiceTypes DefaultServiceType { get; set; }

    public IEnumerable<XConnectServiceTypes> GetServiceTypes() => this._uris.Select<KeyValuePair<XConnectServiceTypes, Uri>, XConnectServiceTypes>((Func<KeyValuePair<XConnectServiceTypes, Uri>, XConnectServiceTypes>) (d => d.Key));

    public IEnumerable<XConnectServiceTypes> GetConnectionStringTypes() => this._uris.Select<KeyValuePair<XConnectServiceTypes, Uri>, XConnectServiceTypes>((Func<KeyValuePair<XConnectServiceTypes, Uri>, XConnectServiceTypes>) (d => d.Key));

    public Uri GetServiceUri(XConnectServiceTypes serviceType, bool doNotUseDefaultAsFallback = false)
    {
      if (this._uris.ContainsKey(serviceType))
      {
        Uri uri = this._uris[serviceType];
        if (uri != (Uri) null)
          return uri;
      }
      return doNotUseDefaultAsFallback || serviceType == this.DefaultServiceType ? (Uri) null : this.GetServiceUri(this.DefaultServiceType, true);
    }

    public void AddServiceUri(XConnectServiceTypes serviceType, Uri uri) => this._uris[serviceType] = uri;

    public IEnumerable<IHttpClientModifier> GetClientModifiers(
      XConnectServiceTypes serviceType)
    {
      return this.GetMembersFromDictionary<IHttpClientModifier>(this._clientModifiers, serviceType);
    }

    public void AddClientModifier(XConnectServiceTypes serviceType, IHttpClientModifier modifier) => this.AddMemberToDictionary<IHttpClientModifier>(this._clientModifiers, serviceType, modifier);

    public IEnumerable<IHttpClientHandlerModifier> GetRequestHandlerModifiers(
      XConnectServiceTypes serviceType)
    {
      return this.GetMembersFromDictionary<IHttpClientHandlerModifier>(this._requestHandlerModifiers, serviceType);
    }

    public void AddRequestHandlerModifier(
      XConnectServiceTypes serviceType,
      IHttpClientHandlerModifier modifier)
    {
      this.AddMemberToDictionary<IHttpClientHandlerModifier>(this._requestHandlerModifiers, serviceType, modifier);
    }

    private IEnumerable<TMember> GetMembersFromDictionary<TMember>(
      IDictionary<XConnectServiceTypes, ICollection<TMember>> dictionary,
      XConnectServiceTypes serviceType)
    {
      if (dictionary != null && dictionary.ContainsKey(serviceType))
      {
        ICollection<TMember> membersFromDictionary = dictionary[serviceType];
        if (membersFromDictionary != null && membersFromDictionary.Count > 0)
          return (IEnumerable<TMember>) membersFromDictionary;
      }
      return Enumerable.Empty<TMember>();
    }

    private void AddMemberToDictionary<TMember>(
      IDictionary<XConnectServiceTypes, ICollection<TMember>> dictionary,
      XConnectServiceTypes serviceType,
      TMember member)
    {
      if (!dictionary.ContainsKey(serviceType))
        dictionary[serviceType] = (ICollection<TMember>) new List<TMember>();
      if (dictionary[serviceType].Contains(member))
        return;
      dictionary[serviceType].Add(member);
    }
  }
}
