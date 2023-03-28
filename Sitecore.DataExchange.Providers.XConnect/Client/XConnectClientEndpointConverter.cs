// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Client.XConnectClientEndpointConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Hashing;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Client
{
  [SupportedIds(new string[] {"{1CE2EDC5-EC66-469B-8697-9E4985CC65CD}"})]
  public class XConnectClientEndpointConverter : BaseEndpointConverter
  {
    public const string FieldNameAllowInvalidCertificates = "AllowInvalidCertificates";
    public const string FieldNameUseCollectionSettingsForAll = "UseCollectionServiceSettingsForAllServices";
    public const string FieldNameCollectionModel = "CollectionModel";
    public const string FieldNameCollectionCSName = "CollectionServiceConnectionStringName";
    public const string FieldNameCollectionCertCSName = "CollectionServiceCertificateConnectionStringName";
    public const string FieldNameCollectionCredCSName = "CollectionServiceCredentialsConnectionStringName";
    public const string FieldNameConfigurationCSName = "ConfigurationServiceConnectionStringName";
    public const string FieldNameConfigurationCertCSName = "ConfigurationServiceCertificateConnectionStringName";
    public const string FieldNameConfigurationCredCSName = "ConfigurationServiceCredentialsConnectionStringName";
    public const string FieldNameSearchCSName = "SearchServiceConnectionStringName";
    public const string FieldNameSearchCertCSName = "SearchServiceCertificateConnectionStringName";
    public const string FieldNameSearchCredCSName = "SearchServiceCredentialsConnectionStringName";
    public const string FieldNameCollectionServiceTimeout = "CollectionServiceTimeout";
    private static IXConnectClientHelper _clientHelper = (IXConnectClientHelper) new DefaultXConnectClientHelper();
    private static HashGenerator _hashGenerator = new HashGenerator();

    public XConnectClientEndpointConverter(IItemModelRepository repository)
      : base(repository)
    {
      this.ClientHelper = XConnectClientEndpointConverter._clientHelper;
      this.HashGenerator = XConnectClientEndpointConverter._hashGenerator;
    }

    public IXConnectClientHelper ClientHelper { get; set; }

    public HashGenerator HashGenerator { get; set; }

    protected override void AddPlugins(ItemModel source, Endpoint endpoint) => this.AddClientSettings(source, endpoint);

    protected virtual void AddClientSettings(ItemModel source, Endpoint endpoint)
    {
      XConnectClientEndpointSettings newPlugin = new XConnectClientEndpointSettings(this.ClientHelper)
      {
        ClientSettings = this.CreateClientSettings(source, endpoint)
      };
      endpoint.AddPlugin<XConnectClientEndpointSettings>(newPlugin);
    }

    protected virtual XConnectClientSettings CreateClientSettings(
      ItemModel source,
      Endpoint endpoint)
    {
      XConnectClientSettings settings = new XConnectClientSettings(source["ItemID"].ToString(), this.HashGenerator.ComputeETag((object) source))
      {
        UseServiceSettingsForAllServices = this.GetBoolValue(source, "UseCollectionServiceSettingsForAllServices")
      };
      if (settings.UseServiceSettingsForAllServices)
        settings.DefaultServiceType = XConnectServiceTypes.CollectionService;
      IDictionary<XConnectServiceTypes, IDictionary<XConnectConnectionStringTypes, string>> stringFieldNames = this.GetConnectionStringFieldNames();
      foreach (XConnectServiceTypes key in (IEnumerable<XConnectServiceTypes>) stringFieldNames.Keys)
        this.AddServiceSettings(key, stringFieldNames[key], settings, source);
      XdbModel model = this.ConvertReferenceToModel<XdbModel>(source, "CollectionModel");
      if (model == null || settings == null)
        return (XConnectClientSettings) null;
      settings.CollectionModel = model;
      int intValue = this.GetIntValue(source, "CollectionServiceTimeout");
      if (intValue > 0 && intValue < 3600)
        settings.AddClientModifier(XConnectServiceTypes.CollectionService, (IHttpClientModifier) new TimeoutHttpClientModifier(new TimeSpan(0, 0, intValue)));
      return settings;
    }

    protected virtual IDictionary<XConnectServiceTypes, IDictionary<XConnectConnectionStringTypes, string>> GetConnectionStringFieldNames() => (IDictionary<XConnectServiceTypes, IDictionary<XConnectConnectionStringTypes, string>>) new Dictionary<XConnectServiceTypes, IDictionary<XConnectConnectionStringTypes, string>>()
    {
      [XConnectServiceTypes.CollectionService] = (IDictionary<XConnectConnectionStringTypes, string>) new Dictionary<XConnectConnectionStringTypes, string>()
      {
        [XConnectConnectionStringTypes.Service] = "CollectionServiceConnectionStringName",
        [XConnectConnectionStringTypes.Certificate] = "CollectionServiceCertificateConnectionStringName",
        [XConnectConnectionStringTypes.Credentials] = "CollectionServiceCredentialsConnectionStringName"
      },
      [XConnectServiceTypes.SearchService] = (IDictionary<XConnectConnectionStringTypes, string>) new Dictionary<XConnectConnectionStringTypes, string>()
      {
        [XConnectConnectionStringTypes.Service] = "SearchServiceConnectionStringName",
        [XConnectConnectionStringTypes.Certificate] = "SearchServiceCertificateConnectionStringName",
        [XConnectConnectionStringTypes.Credentials] = "SearchServiceCredentialsConnectionStringName"
      },
      [XConnectServiceTypes.ConfigurationService] = (IDictionary<XConnectConnectionStringTypes, string>) new Dictionary<XConnectConnectionStringTypes, string>()
      {
        [XConnectConnectionStringTypes.Service] = "ConfigurationServiceConnectionStringName",
        [XConnectConnectionStringTypes.Certificate] = "ConfigurationServiceCertificateConnectionStringName",
        [XConnectConnectionStringTypes.Credentials] = "ConfigurationServiceCredentialsConnectionStringName"
      }
    };

    protected virtual void AddServiceSettings(
      XConnectServiceTypes serviceType,
      IDictionary<XConnectConnectionStringTypes, string> connectionStrings,
      XConnectClientSettings settings,
      ItemModel source)
    {
      Uri connectionStringAsUri = this.GetConnectionStringAsUri(source, connectionStrings[XConnectConnectionStringTypes.Service]);
      if (connectionStringAsUri != (Uri) null)
        settings.AddServiceUri(serviceType, connectionStringAsUri);
      IHttpClientHandlerModifier modifierForCertificate = this.GetRequestHandlerModifierForCertificate(source, connectionStrings[XConnectConnectionStringTypes.Certificate]);
      if (modifierForCertificate != null)
        settings.AddRequestHandlerModifier(serviceType, modifierForCertificate);
      IHttpClientHandlerModifier modifierForCredentials = this.GetRequestHandlerModifierForCredentials(source, connectionStrings[XConnectConnectionStringTypes.Credentials]);
      if (modifierForCredentials == null)
        return;
      settings.AddRequestHandlerModifier(serviceType, modifierForCredentials);
    }

    protected virtual IHttpClientHandlerModifier GetRequestHandlerModifierForCertificate(
      ItemModel source,
      string fieldName)
    {
      string connectionStringValue = this.GetConnectionStringValue(source, fieldName);
      if (string.IsNullOrWhiteSpace(connectionStringValue))
        return (IHttpClientHandlerModifier) null;
      CertificateHttpClientHandlerModifierOptions options = CertificateHttpClientHandlerModifierOptions.Parse(connectionStringValue);
      if (options == null)
        return (IHttpClientHandlerModifier) null;
      if (this.GetBoolValue(source, "AllowInvalidCertificates"))
        options.AllowInvalidClientCertificates = "true";
      return (IHttpClientHandlerModifier) new CertificateHttpClientHandlerModifier(options);
    }

    protected virtual IHttpClientHandlerModifier GetRequestHandlerModifierForCredentials(
      ItemModel source,
      string fieldName)
    {
      string connectionStringValue = this.GetConnectionStringValue(source, fieldName);
      return string.IsNullOrWhiteSpace(connectionStringValue) ? (IHttpClientHandlerModifier) null : this.GetRequestHandlerModifierForCredentials(CredentialsWebRequestHandlerModifierOptions.Parse(connectionStringValue));
    }

    protected virtual IHttpClientHandlerModifier GetRequestHandlerModifierForCredentials(
      CredentialsWebRequestHandlerModifierOptions options)
    {
      if (options == null)
        return (IHttpClientHandlerModifier) null;
      return string.Equals("basic", options.Mode, StringComparison.InvariantCultureIgnoreCase) ? (IHttpClientHandlerModifier) new NetworkCredentialsWebRequestHandlerModifier(options.UserName, options.Password) : (IHttpClientHandlerModifier) null;
    }

    private Uri GetConnectionStringAsUri(ItemModel source, string fieldName)
    {
      string connectionStringValue = this.GetConnectionStringValue(source, fieldName);
      return string.IsNullOrWhiteSpace(connectionStringValue) ? (Uri) null : new Uri(connectionStringValue);
    }
  }
}
