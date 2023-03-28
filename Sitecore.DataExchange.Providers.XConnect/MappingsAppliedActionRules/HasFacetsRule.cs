// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActionRules.HasFacetsRule
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActionRules
{
  public class HasFacetsRule : IMappingsAppliedActionRule
  {
    public bool ShouldActionBeRun(IMappingsAppliedAction action, MappingsAppliedContext context) => context.MappingContext.Target is EntityModel target && target.Facets.Any<KeyValuePair<string, Facet>>();
  }
}
