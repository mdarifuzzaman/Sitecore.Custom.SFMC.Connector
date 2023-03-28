// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.FilterExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  [SupportedIds(new string[] {"{D63C5868-335A-407B-A81E-02C57B7681E6}"})]
  public class FilterExpressionConverter : BaseItemModelConverter<FilterExpressionDescriptor>
  {
    public const string FieldNameLogicalOperator = "LogicalOperator";

    public FilterExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<FilterExpressionDescriptor> ConvertSupportedItem(
      ItemModel source)
    {
      FilterExpressionDescriptor expressionDescriptor = new FilterExpressionDescriptor()
      {
        LogicalOperator = this.GetStringValue(source, "LogicalOperator")
      };
      foreach (ConditionExpressionDescriptor model in this.ConvertChildrenToModels<ConditionExpressionDescriptor>(source))
        expressionDescriptor.ConditionExpressions.Add(model);
      return this.PositiveResult(expressionDescriptor);
    }
  }
}
