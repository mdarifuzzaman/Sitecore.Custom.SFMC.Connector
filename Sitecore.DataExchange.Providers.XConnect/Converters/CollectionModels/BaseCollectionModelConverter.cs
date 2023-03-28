// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels.BaseCollectionModelConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.XConnect.Schema;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels
{
  public abstract class BaseCollectionModelConverter : BaseItemModelConverter<XdbModel>
  {
    protected BaseCollectionModelConverter(IItemModelRepository repository)
      : base(repository)
    {
    }
  }
}
