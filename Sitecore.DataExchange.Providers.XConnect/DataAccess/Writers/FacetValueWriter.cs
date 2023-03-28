// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.FacetValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class FacetValueWriter : IValueWriter
  {
    public FacetValueWriter(string facetName) => this.FacetName = facetName;

    public string FacetName { get; protected set; }

    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      if (!this.CanWrite(target, value, context))
        return false;
      EntityModel entityModel = target as EntityModel;
      Facet facet = value as Facet;
      if (entityModel == null || facet == null)
        return false;
      entityModel.SetFacet(this.FacetName, facet);
      return true;
    }

    protected virtual bool CanWrite(object target, object value, DataAccessContext context) => !string.IsNullOrWhiteSpace(this.FacetName) && target is EntityModel && value is Facet;
  }
}
