// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ReferenceDataClientEndpointSettings
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.Xdb.Common.Web;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  public class ReferenceDataClientEndpointSettings : IPlugin
  {
    public ReferenceDataClientEndpointSettings(Uri baseAddress) => this.BaseAddress = baseAddress;

    public Uri BaseAddress { get; private set; }

    public IHttpClientHandlerModifier[] Modifiers { get; set; }
  }
}
