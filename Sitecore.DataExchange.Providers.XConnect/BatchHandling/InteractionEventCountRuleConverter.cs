// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.BatchHandling.InteractionEventCountRuleConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.BatchHandling
{
  [SupportedIds(new string[] {"{7944CC2F-F8EC-4F30-9146-BF021D6B6292}"})]
  public class InteractionEventCountRuleConverter : BaseAddObjectToBatchRuleConverter
  {
    public const string FieldNameCount = "Count";
    public const string FieldNameOperator = "Operator";

    public InteractionEventCountRuleConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IAddObjectToBatchRule> ConvertSupportedItem(
      ItemModel source)
    {
      Func<InteractionModel, int, bool> comparisonDelegate = this.GetComparisonDelegate(source);
      if (comparisonDelegate == null)
        return this.NegativeResult(source, "No interaction event count comparison delegate was resolved.");
      return this.PositiveResult((IAddObjectToBatchRule) new InteractionEventCountRule()
      {
        Identifier = source.GetItemId().ToString(),
        Count = this.GetIntValue(source, "Count"),
        Compare = comparisonDelegate
      });
    }

    protected virtual Func<InteractionModel, int, bool> GetComparisonDelegate(
      ItemModel source)
    {
      Guid guidValue = this.GetGuidValue(source, "Operator");
      if (guidValue == ItemIDs.NumericConditionOperatorsEqual)
        return (Func<InteractionModel, int, bool>) ((model, count) => model != null && model.Events.Count == count);
      if (guidValue == ItemIDs.NumericConditionOperatorsNotEqual)
        return (Func<InteractionModel, int, bool>) ((model, count) => model != null && model.Events.Count != count);
      if (guidValue == ItemIDs.NumericConditionOperatorsLessThan)
        return (Func<InteractionModel, int, bool>) ((model, count) => model != null && model.Events.Count < count);
      if (guidValue == ItemIDs.NumericConditionOperatorsLessThanOrEqualTo)
        return (Func<InteractionModel, int, bool>) ((model, count) => model != null && model.Events.Count <= count);
      if (guidValue == ItemIDs.NumericConditionOperatorsGreaterThan)
        return (Func<InteractionModel, int, bool>) ((model, count) => model != null && model.Events.Count > count);
      return guidValue == ItemIDs.NumericConditionOperatorsGreaterOrThanEqualTo ? (Func<InteractionModel, int, bool>) ((model, count) => model != null && model.Events.Count >= count) : (Func<InteractionModel, int, bool>) null;
    }
  }
}
