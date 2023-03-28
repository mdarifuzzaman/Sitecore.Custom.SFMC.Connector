// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EntityFacetPropertyWithMappingValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{FB0DC918-A640-4DD9-BC77-79818BDCCB70}"})]
  public class EntityFacetPropertyWithMappingValueAccessorConverter : 
    EntityFacetPropertyValueAccessorConverter
  {
    public const string FieldNameMappingSet = "MappingSet";

    public EntityFacetPropertyWithMappingValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      ItemModel facetPropertyItem = this.GetFacetPropertyItem(source);
      return (IValueWriter) new FacetPropertyWithMappingValueWriter(this.GetFacetName(facetPropertyItem), this.GetFacetPropertyName(facetPropertyItem), this.ConvertReferenceToModel<IMappingSet>(source, "MappingSet"));
    }
  }
}
