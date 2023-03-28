// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Client.BaseXConnectServiceClientConverter`1
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Serialization;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Client
{
  public abstract class BaseXConnectServiceClientConverter<TTo> : 
    IConverter<XConnectClientSettings, TTo>
  {
    private static IDictionary<string, XConnectClientConfiguration> _configs = (IDictionary<string, XConnectClientConfiguration>) new Dictionary<string, XConnectClientConfiguration>();
    private static IDictionary<string, string> _hashes = (IDictionary<string, string>) new Dictionary<string, string>();

    public virtual ConvertResult<TTo> Convert(XConnectClientSettings settings)
    {
      if (settings == null || settings.CollectionModel == null || settings.CollectionModel.FullName == null || string.IsNullOrWhiteSpace(settings.SettingsIdentifier))
        return ConvertResult<TTo>.NegativeResult();
      XConnectClientConfiguration config = (XConnectClientConfiguration) null;
      if (this.ShouldUseCurrentClientConfiguration(settings))
        config = BaseXConnectServiceClientConverter<TTo>._configs[settings.SettingsIdentifier];
      if (config == null)
        config = this.CreateNewClientConfiguration(settings);
      return config == null ? ConvertResult<TTo>.NegativeResult() : ConvertResult<TTo>.PositiveResult(this.Convert(config));
    }

    protected virtual bool ShouldUseCurrentClientConfiguration(XConnectClientSettings settings)
    {
      if (settings == null || !BaseXConnectServiceClientConverter<TTo>._hashes.ContainsKey(settings.SettingsIdentifier))
        return false;
      string hash = BaseXConnectServiceClientConverter<TTo>._hashes[settings.SettingsIdentifier];
      string settingsHash = settings.SettingsHash;
      return !string.IsNullOrWhiteSpace(hash) && (string.IsNullOrWhiteSpace(settingsHash) || hash == settingsHash);
    }

    protected virtual XConnectClientConfiguration CreateNewClientConfiguration(
      XConnectClientSettings settings)
    {
      XConnectClientConfiguration clientConfiguration = this.GetClientConfiguration(settings);
      if (clientConfiguration == null)
        return (XConnectClientConfiguration) null;
      clientConfiguration.Initialize();
      BaseXConnectServiceClientConverter<TTo>._configs[settings.SettingsIdentifier] = clientConfiguration;
      BaseXConnectServiceClientConverter<TTo>._hashes[settings.SettingsIdentifier] = settings.SettingsHash;
      return clientConfiguration;
    }

    protected abstract TTo Convert(XConnectClientConfiguration config);

    protected virtual XConnectClientConfiguration GetClientConfiguration(
      XConnectClientSettings settings)
    {
      if (settings == null)
        return (XConnectClientConfiguration) null;
      return this.IsSimpleMode(settings) ? this.GetSimpleConfiguration(settings) : this.GetAdvancedConfiguration(settings);
    }

    protected virtual bool IsSimpleMode(XConnectClientSettings settings)
    {
      if (settings.GetServiceTypes().Count<XConnectServiceTypes>((Func<XConnectServiceTypes, bool>) (t => t == settings.DefaultServiceType)) != 1 || !settings.UseServiceSettingsForAllServices && settings.GetServiceTypes().Count<XConnectServiceTypes>((Func<XConnectServiceTypes, bool>) (t => t != settings.DefaultServiceType)) > 0)
        return false;
      IEnumerable<XConnectServiceTypes> xconnectServiceTypeses;
      if (settings.UseServiceSettingsForAllServices)
        xconnectServiceTypeses = (IEnumerable<XConnectServiceTypes>) new XConnectServiceTypes[1]
        {
          settings.DefaultServiceType
        };
      else
        xconnectServiceTypeses = Enum.GetValues(typeof (XConnectServiceTypes)).Cast<XConnectServiceTypes>();
      foreach (XConnectServiceTypes serviceType in xconnectServiceTypeses)
      {
        if (settings.GetClientModifiers(serviceType).Any<IHttpClientModifier>() || settings.GetRequestHandlerModifiers(serviceType).Any<IHttpClientHandlerModifier>())
          return false;
      }
      return true;
    }

    protected virtual XConnectClientConfiguration GetSimpleConfiguration(
      XConnectClientSettings settings)
    {
      if (settings == null || settings.CollectionModel == null)
        return (XConnectClientConfiguration) null;
      Uri serviceUri = settings.GetServiceUri(settings.DefaultServiceType);
      return serviceUri == (Uri) null ? (XConnectClientConfiguration) null : new XConnectClientConfiguration(settings.CollectionModel, serviceUri, serviceUri, true);
    }

    protected virtual XConnectClientConfiguration GetAdvancedConfiguration(
      XConnectClientSettings settings)
    {
      if (settings == null || settings.CollectionModel == null)
        return (XConnectClientConfiguration) null;
      XConnectServiceTypes serviceType1 = XConnectServiceTypes.CollectionService;
      XConnectServiceTypes serviceType2 = XConnectServiceTypes.SearchService;
      XConnectServiceTypes serviceType3 = XConnectServiceTypes.ConfigurationService;
      if (settings.UseServiceSettingsForAllServices)
      {
        serviceType1 = settings.DefaultServiceType;
        serviceType2 = settings.DefaultServiceType;
        serviceType3 = settings.DefaultServiceType;
      }
      CollectionWebApiClient collectionService = this.GetWebApiClientForCollectionService(serviceType1, settings);
      SearchWebApiClient forSearchService = this.GetWebApiClientForSearchService(serviceType2, settings);
      IExternalXdbModelResolver configurationService = this.GetWebApiClientForConfigurationService(serviceType3, settings);
      return new XConnectClientConfiguration(settings.CollectionModel, collectionService, forSearchService, configurationService, true);
    }

    protected virtual CollectionWebApiClient GetWebApiClientForCollectionService(
      XConnectServiceTypes serviceType,
      XConnectClientSettings settings)
    {
      return new CollectionWebApiClient(new Uri(settings.GetServiceUri(serviceType), "odata/"), settings.GetClientModifiers(serviceType), settings.GetRequestHandlerModifiers(serviceType));
    }

    protected virtual SearchWebApiClient GetWebApiClientForSearchService(
      XConnectServiceTypes serviceType,
      XConnectClientSettings settings)
    {
      return new SearchWebApiClient(new Uri(settings.GetServiceUri(serviceType), "odata/"), settings.GetClientModifiers(serviceType), settings.GetRequestHandlerModifiers(serviceType));
    }

    protected virtual IExternalXdbModelResolver GetWebApiClientForConfigurationService(
      XConnectServiceTypes serviceType,
      XConnectClientSettings settings)
    {
      return (IExternalXdbModelResolver) new ConfigurationWebApiClient(new Uri(settings.GetServiceUri(serviceType), "configuration/"), settings.GetClientModifiers(serviceType), settings.GetRequestHandlerModifiers(serviceType));
    }
  }
}
