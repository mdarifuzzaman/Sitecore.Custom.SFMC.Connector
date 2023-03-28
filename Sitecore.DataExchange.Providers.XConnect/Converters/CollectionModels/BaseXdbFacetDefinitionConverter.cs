// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels.BaseXdbFacetDefinitionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels
{
  public abstract class BaseXdbFacetDefinitionConverter : BaseItemModelConverter<XdbFacetDefinition>
  {
    public const string FieldNameEntityType = "EntityType";
    public const string FieldNameFacetName = "FacetName";
    public const string FacetTypeFacetType = "FacetType";
    public const string FieldNameCollectionModelType = "CollectionModelType";

    protected BaseXdbFacetDefinitionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<XdbFacetDefinition> ConvertSupportedItem(
      ItemModel source)
    {
      EntityType valueFromReference = this.GetEnumValueFromReference<EntityType>(source, "EntityType", "ItemName");
      string stringValue = this.GetStringValue(source, "FacetName");
      if (string.IsNullOrWhiteSpace(stringValue))
        return this.NegativeResult(source, "No facet name is specified on the item.");
      Type typeFromTypeName1 = this.GetTypeFromTypeName(source, "FacetType");
      if (typeFromTypeName1 == (Type) null)
        return this.NegativeResult(source, "No facet type is specified on the item.");
      XdbFacetType facetType = new XdbFacetType(typeFromTypeName1.Namespace, typeFromTypeName1.Name, typeFromTypeName1.IsAbstract, (XdbObjectType) null);
      XdbFacetDefinition facet = new XdbFacetDefinition(valueFromReference, stringValue, facetType);
      Type typeFromTypeName2 = this.GetTypeFromTypeName(source, "CollectionModelType");
      if (typeFromTypeName2 != (Type) null)
      {
        XdbModel xdbModel = typeFromTypeName2.GetXdbModel();
        if (xdbModel != null)
        {
          XdbFacetDefinition xdbFacetDefinition = xdbModel.Facets.FirstOrDefault<XdbFacetDefinition>((Func<XdbFacetDefinition, bool>) (f => f.Name == facet.Name && f.Target == facet.Target));
          if (xdbFacetDefinition != null)
            return this.PositiveResult(xdbFacetDefinition);
        }
      }
      return this.PositiveResult(facet);
    }
  }
}
