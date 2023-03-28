// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.ApplyMappingRules.TargetCollectionNonEmptyRuleConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.Salesforce.DataAccess.ApplyMappingRules;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.ApplyMappingRules
{
  [SupportedIds(new string[] {"{801BE33D-C611-410D-8013-A8D354A5AECC}"})]
  public class TargetCollectionNonEmptyRuleConverter : BaseItemModelConverter<IApplyMappingRule>
  {
    private static TargetCollectionNonEmptyMappingRule _rule = new TargetCollectionNonEmptyMappingRule();

    public TargetCollectionNonEmptyRuleConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IApplyMappingRule> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IApplyMappingRule) TargetCollectionNonEmptyRuleConverter._rule);
    }
  }
}
