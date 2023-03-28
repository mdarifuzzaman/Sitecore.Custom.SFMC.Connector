// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActionRules.HasFacetsRuleConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters.DataAccess.MappingsAppliedActionRules;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.MappingsAppliedActionRules
{
  public class HasFacetsRuleConverter : BaseMappingsAppliedActionRuleConverter<HasFacetsRule>
  {
    public HasFacetsRuleConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override HasFacetsRule CreateInstance(ItemModel source) => new HasFacetsRule();
  }
}
