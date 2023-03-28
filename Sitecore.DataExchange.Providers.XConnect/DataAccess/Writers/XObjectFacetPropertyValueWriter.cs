// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.XObjectFacetPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class XObjectFacetPropertyValueWriter : XObjectPropertyValueWriter
  {
    public XObjectFacetPropertyValueWriter(
      string facetName,
      string propertyName,
      EntityType entityType,
      XdbModel xdbModel,
      IMappingSet mappingSet)
      : base(propertyName, entityType, xdbModel, mappingSet)
    {
      Condition.Requires<string>(facetName).IsNotNullOrWhiteSpace();
      this.FacetName = facetName;
    }

    public string FacetName { get; private set; }
  }
}
