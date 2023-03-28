// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.FilterExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  [SupportedIds(new string[] {"{055EFC9E-F356-4481-8E0C-50FB5FF8112F}"})]
  public class FilterExpressionConverter : BaseItemModelConverter<IEntityExpressionBuilder>
  {
    public const string FieldNameLogicalOperator = "LogicalOperator";

    public FilterExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      FilterExpressionBuilder expressionBuilder = new FilterExpressionBuilder()
      {
        LogicalOperator = this.GetStringValue(source, "LogicalOperator")
      };
      foreach (IEntityExpressionBuilder model in this.ConvertChildrenToModels<IEntityExpressionBuilder>(source))
        expressionBuilder.ConditionExpressionBuilders.Add(model);
      return this.PositiveResult((IEntityExpressionBuilder) expressionBuilder);
    }
  }
}
