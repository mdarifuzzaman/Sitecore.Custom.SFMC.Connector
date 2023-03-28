// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ReferenceDataClientEndpointConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.Xdb.Common.Web;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [SupportedIds(new string[] {"{5DFEC6E6-E5A4-4514-97E4-2E79CCDE9365}"})]
  public class ReferenceDataClientEndpointConverter : BaseEndpointConverter
  {
    public const string FieldNameAllowInvalidCertificates = "AllowInvalidCertificates";
    public const string FieldNameClientConnectionStringName = "ClientConnectionStringName";
    public const string FieldNameClientCertificateConnectionStringName = "ClientCertificateConnectionStringName";

    public ReferenceDataClientEndpointConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, Endpoint endpoint) => this.AddClientPlugin(source, endpoint);

    protected virtual void AddClientPlugin(ItemModel source, Endpoint endpoint)
    {
      Uri baseAddress = this.GetBaseAddress(source, endpoint);
      IHttpClientHandlerModifier[] modifiers = this.GetModifiers(source, endpoint);
      ReferenceDataClientEndpointSettings newPlugin = new ReferenceDataClientEndpointSettings(baseAddress)
      {
        Modifiers = modifiers
      };
      endpoint.AddPlugin<ReferenceDataClientEndpointSettings>(newPlugin);
    }

    protected virtual Uri GetBaseAddress(ItemModel source, Endpoint endpoint)
    {
      string connectionStringValue = this.GetConnectionStringValue(source, "ClientConnectionStringName");
      if (string.IsNullOrWhiteSpace(connectionStringValue))
        return (Uri) null;
      Uri result = (Uri) null;
      return !Uri.TryCreate(connectionStringValue, UriKind.Absolute, out result) ? (Uri) null : result;
    }

    protected virtual IHttpClientHandlerModifier[] GetModifiers(
      ItemModel source,
      Endpoint endpoint)
    {
      string connectionStringValue = this.GetConnectionStringValue(source, "ClientCertificateConnectionStringName");
      if (string.IsNullOrWhiteSpace(connectionStringValue))
        return (IHttpClientHandlerModifier[]) null;
      CertificateHttpClientHandlerModifierOptions options = CertificateHttpClientHandlerModifierOptions.Parse(connectionStringValue);
      if (this.GetBoolValue(source, "AllowInvalidCertificates"))
        options.AllowInvalidClientCertificates = "true";
      return new IHttpClientHandlerModifier[1]
      {
        (IHttpClientHandlerModifier) new CertificateHttpClientHandlerModifier(options)
      };
    }
  }
}
