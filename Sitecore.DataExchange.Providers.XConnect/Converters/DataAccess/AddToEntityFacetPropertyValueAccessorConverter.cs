// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.AddToEntityFacetPropertyValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  public class AddToEntityFacetPropertyValueAccessorConverter : 
    EntityFacetPropertyValueAccessorConverter
  {
    public AddToEntityFacetPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      IValueWriter model = this.ConvertReferenceToModel<IValueWriter>(source, "ValueWriter");
      if (model != null)
        return model;
      ItemModel facetPropertyItem = this.GetFacetPropertyItem(source);
      if (facetPropertyItem == null)
        return (IValueWriter) null;
      return this.IsFacetPropertyReadOnly(facetPropertyItem) ? (IValueWriter) null : (IValueWriter) new AddToFacetPropertyListValueWriter(this.GetFacetName(facetPropertyItem), this.GetFacetPropertyName(facetPropertyItem));
    }
  }
}
