// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EntityFacetPropertyValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{8A140187-897F-4C90-8024-46DC953F9344}"})]
  public class EntityFacetPropertyValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameEntityType = "EntityType";
    public const string FieldNamePropertyName = "PropertyName";
    public const string FieldNameReadOnly = "ReadOnly";
    public const string FieldNameFacetName = "FacetName";
    public const string FieldNameFacetType = "FacetType";
    public const string FieldNameCollectionModelType = "CollectionModelType";
    public const string FieldNameFacetProperty = "FacetProperty";

    public EntityFacetPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueReader GetValueReader(ItemModel source)
    {
      IValueReader valueReader = base.GetValueReader(source);
      if (valueReader != null)
        return valueReader;
      ItemModel facetPropertyItem = this.GetFacetPropertyItem(source);
      return facetPropertyItem == null ? (IValueReader) null : (IValueReader) new FacetPropertyValueReader(this.GetFacetName(facetPropertyItem), this.GetFacetPropertyName(facetPropertyItem));
    }

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      IValueWriter valueWriter = base.GetValueWriter(source);
      if (valueWriter != null)
        return valueWriter;
      ItemModel facetPropertyItem = this.GetFacetPropertyItem(source);
      if (facetPropertyItem == null)
        return (IValueWriter) null;
      return this.IsFacetPropertyReadOnly(facetPropertyItem) ? (IValueWriter) null : (IValueWriter) new FacetPropertyValueWriter(this.GetFacetName(facetPropertyItem), this.GetFacetPropertyName(facetPropertyItem));
    }

    protected virtual ItemModel GetFacetPropertyItem(ItemModel source) => this.GetReferenceAsModel(source, "FacetProperty");

    protected virtual string GetFacetName(ItemModel hasFacetSettingsItem) => this.GetStringValue(hasFacetSettingsItem, "FacetName");

    protected virtual string GetFacetPropertyName(ItemModel facetPropertyItem) => this.GetStringValue(facetPropertyItem, "PropertyName");

    protected virtual bool IsFacetPropertyReadOnly(ItemModel facetPropertyItem) => this.GetBoolValue(facetPropertyItem, "ReadOnly");

    protected virtual EntityType GetEntityType(ItemModel hasFacetSettingsItem) => this.GetEnumValueFromReference<EntityType>(hasFacetSettingsItem, "EntityType", "ItemName");

    protected virtual Type GetCollectionModelType(ItemModel hasFacetSettingsItem) => this.GetTypeFromTypeName(hasFacetSettingsItem, "CollectionModelType");
  }
}
