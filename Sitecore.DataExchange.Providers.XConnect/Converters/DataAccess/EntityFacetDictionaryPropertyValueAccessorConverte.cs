// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EntityFacetDictionaryPropertyValueAccessorConverter
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
  [SupportedIds(new string[] {"{72D0FCB8-E0B4-4655-9DEC-3E290182BF50}"})]
  public class EntityFacetDictionaryPropertyValueAccessorConverter : 
    EntityFacetPropertyValueAccessorConverter
  {
    public const string FieldNameMappingSet = "MappingSet";

    public EntityFacetDictionaryPropertyValueAccessorConverter(IItemModelRepository repository)
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
      return (IValueWriter) new FacetDictionaryPropertyValueWriter(facetName, facetPropertyName, entityType, xdbModel, model);
    }
  }
}
