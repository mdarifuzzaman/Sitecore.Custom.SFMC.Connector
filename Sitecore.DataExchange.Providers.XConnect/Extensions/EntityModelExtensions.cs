// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Extensions.EntityModelExtensions
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Extensions
{
  public static class EntityModelExtensions
  {
    public static Facet GetFacet(this EntityModel entityModel, string facetName) => !entityModel.HasFacet(facetName) ? (Facet) null : entityModel.Facets[facetName];

    public static bool HasFacet(this EntityModel entityModel, string facetName) => entityModel.Facets.ContainsKey(facetName);

    public static void SetFacet(this EntityModel entityModel, string facetName, Facet facet) => entityModel.Facets[facetName] = facet;
  }
}
