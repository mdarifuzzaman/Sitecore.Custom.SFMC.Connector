// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Plugins.ReadEntitySettings
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Expressions;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Plugins
{
  public class ReadEntitySettings : IPlugin
  {
    public readonly int DefaultBatchSize = 100;

    public int BatchSize { get; set; }

    public int MaxCount { get; set; }

    public List<string> FacetNames { get; set; }

    public IEntityExpressionBuilder EntityExpressionBuilder { get; set; }
  }
}
