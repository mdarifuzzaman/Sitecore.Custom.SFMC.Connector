// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActions.RemoveFacetsActionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters.DataAccess.MappingsAppliedActions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActions
{
  public class RemoveFacetsActionConverter : BaseMappingsAppliedActionConverter<RemoveFacetsAction>
  {
    public RemoveFacetsActionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override RemoveFacetsAction CreateInstance(ItemModel source) => new RemoveFacetsAction();
  }
}
