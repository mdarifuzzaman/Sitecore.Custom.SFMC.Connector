// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.AlwaysTrueConditionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  [SupportedIds(new string[] {"{69E64E24-B86E-478F-B8AF-7E8BF2CBD47A}"})]
  public class AlwaysTrueConditionConverter : BaseXObjectMappingConditionConverter
  {
    private static AlwaysTrueCondition _condition = new AlwaysTrueCondition();

    public AlwaysTrueConditionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IXObjectMappingCondition> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IXObjectMappingCondition) AlwaysTrueConditionConverter._condition);
    }
  }
}
