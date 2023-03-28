// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EntityFacetXObjectPropertyValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{F8AB8CB2-CF29-4C4F-9E30-C4D200E7AF4F}"})]
  public class EntityFacetXObjectPropertyValueAccessorConverter : 
    EntityFacetPropertyValueAccessorConverter
  {
    public const string FieldNameMappingSet = "MappingSet";

    public EntityFacetXObjectPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      ItemModel facetPropertyItem = this.GetFacetPropertyItem(source);
      string facetName = this.GetFacetName(facetPropertyItem);
      string facetPropertyName = this.GetFacetPropertyName(facetPropertyItem);
      EntityType entityType = this.GetEntityType(facetPropertyItem);
      Type collectionModelType = this.GetCollectionModelType(facetPropertyItem);
      if (collectionModelType == (Type) null)
        return (IValueWriter) null;
      XdbModel xdbModel = collectionModelType.GetXdbModel();
      IMappingSet model = this.ConvertReferenceToModel<IMappingSet>(source, "MappingSet");
      return (IValueWriter) new XObjectFacetPropertyValueWriter(facetName, facetPropertyName, entityType, xdbModel, model);
    }
  }
}
