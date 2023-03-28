// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels.ConfiguredCollectionModelConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels
{
  [SupportedIds(new string[] {"{83C8AD49-3E50-4900-836B-1436F3C31064}"})]
  public class ConfiguredCollectionModelConverter : BaseCollectionModelConverter
  {
    public const string FieldNameModelName = "ModelName";
    public const string FieldNameModelVersionMajor = "ModelVersionMajor";
    public const string FieldNameModelVersionMinor = "ModelVersionMinor";
    public const string FieldNameReferencedCollectionModels = "ReferencedCollectionModels";

    public ConfiguredCollectionModelConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<XdbModel> ConvertSupportedItem(
      ItemModel source)
    {
      string stringValue = this.GetStringValue(source, "ModelName");
      XdbModelVersion version = new XdbModelVersion(this.GetIntValue(source, "ModelVersionMajor"), this.GetIntValue(source, "ModelVersionMinor"));
      XdbFacetDefinition[] forCollectionModel1 = this.GetFacetsForCollectionModel(source);
      XdbModel[] forCollectionModel2 = this.GetReferencesForCollectionModel(source);
      XdbNamedType[] forCollectionModel3 = this.GetNamedTypesForCollectionModel(source);
      return this.PositiveResult(new XdbModel(stringValue, version, forCollectionModel3, forCollectionModel1, forCollectionModel2));
    }

    protected virtual XdbNamedType[] GetNamedTypesForCollectionModel(ItemModel source) => new XdbNamedType[0];

    protected virtual XdbFacetDefinition[] GetFacetsForCollectionModel(
      ItemModel source)
    {
      List<XdbFacetDefinition> xdbFacetDefinitionList = new List<XdbFacetDefinition>();
      ItemModel itemModel = this.GetChildItemModels(source).FirstOrDefault<ItemModel>((Func<ItemModel, bool>) (i => i["ItemName"] == (object) "Facets"));
      if (itemModel != null)
        this.GetChildItemModels(itemModel);
      this.ConvertReferencesToModels<XdbModel>(source, "ReferencedCollectionModels");
      return xdbFacetDefinitionList.ToArray();
    }

    protected virtual XdbModel[] GetReferencesForCollectionModel(ItemModel source) => this.ConvertReferencesToModels<XdbModel>(source, "ReferencedCollectionModels").ToArray<XdbModel>();
  }
}
