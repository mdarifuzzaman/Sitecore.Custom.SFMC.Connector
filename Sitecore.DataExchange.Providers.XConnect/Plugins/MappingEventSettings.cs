// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Plugins.MappingEventSettings
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.XConnect;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Plugins
{
  public class MappingEventSettings : IDataAccessPlugin
  {
    public List<XObject> Events { get; set; }
  }
}
