// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Plugins.ResolveEntitySettings
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Plugins
{
  public class ResolveEntitySettings : IPlugin
  {
    public bool UseConnectionSettingsFromContext { get; set; }

    public List<string> FacetNames { get; set; }

    public EntityType EntityType { get; set; }

    public bool DoNotConvertToEntityModel { get; set; }
  }
}
