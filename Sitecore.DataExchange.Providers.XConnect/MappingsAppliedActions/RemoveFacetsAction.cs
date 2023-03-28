// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActions.RemoveFacetsAction
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActions
{
  public class RemoveFacetsAction : IMappingsAppliedAction
  {
    public ICollection<IMappingsAppliedActionRule> MappingsAppliedActionRules { get; set; }

    public bool DoAction(MappingsAppliedContext context)
    {
      if (!this.MappingsAppliedActionRules.All<IMappingsAppliedActionRule>((Func<IMappingsAppliedActionRule, bool>) (r => r.ShouldActionBeRun((IMappingsAppliedAction) this, context))) || !(context.MappingContext.Target is EntityModel target) || !target.Facets.Any<KeyValuePair<string, Facet>>())
        return false;
      target.Facets = (IDictionary<string, Facet>) new Dictionary<string, Facet>();
      return true;
    }
  }
}
