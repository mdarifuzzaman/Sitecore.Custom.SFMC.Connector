// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels.CompiledCollectionModelConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect.Schema;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels
{
  [SupportedIds(new string[] {"{9C6FE1C6-12A5-4C17-84FA-EEFA0E7DA6D7}"})]
  public class CompiledCollectionModelConverter : BaseCollectionModelConverter
  {
    public const string FieldNameCollectionModelType = "CollectionModelType";

    public CompiledCollectionModelConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<XdbModel> ConvertSupportedItem(
      ItemModel source)
    {
      Type typeFromTypeName = this.GetTypeFromTypeName(source, "CollectionModelType");
      return typeFromTypeName == (Type) null ? this.NegativeResult(source, "No collection model type was specified on the item.") : this.PositiveResult(typeFromTypeName.GetXdbModel());
    }
  }
}
