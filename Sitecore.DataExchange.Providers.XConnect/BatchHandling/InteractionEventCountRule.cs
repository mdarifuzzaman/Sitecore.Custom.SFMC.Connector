// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.BatchHandling.InteractionEventCountRule
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Providers.XConnect.Models;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.BatchHandling
{
  public class InteractionEventCountRule : IAddObjectToBatchRule
  {
    public int Count { get; set; }

    public Func<InteractionModel, int, bool> Compare { get; set; }

    public string Identifier { get; set; }

    public virtual bool ShouldAddObjectToBatch(object obj) => obj is InteractionModel interactionModel && this.Compare != null && this.Compare(interactionModel, this.Count);
  }
}
