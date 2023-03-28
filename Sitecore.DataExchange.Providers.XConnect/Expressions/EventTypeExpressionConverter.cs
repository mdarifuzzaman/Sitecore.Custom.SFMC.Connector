// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.EventTypeExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class EventTypeExpressionConverter : BaseItemModelConverter<IEventExpressionBuilder>
  {
    public const string FieldNameConditionOperator = "ConditionOperator";
    public const string FieldNameEventType = "EventType";
    protected readonly ExpressionTypeConverter ExpressionTypeConverter;

    public EventTypeExpressionConverter(IItemModelRepository repository)
      : this(repository, new ExpressionTypeConverter())
    {
    }

    public EventTypeExpressionConverter(
      IItemModelRepository repository,
      ExpressionTypeConverter expressionTypeConverter)
      : base(repository)
    {
      this.ExpressionTypeConverter = expressionTypeConverter ?? new ExpressionTypeConverter();
    }

    protected override ConvertResult<IEventExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      ConvertResult<ExpressionType> convertResult = this.ExpressionTypeConverter.Convert(this.GetConditionOperator(source));
      if (!convertResult.WasConverted)
        return this.NegativeResult(source, "Cannot convert condition operator to expression type. (Reason: " + convertResult.Reason + ")");
      return this.PositiveResult((IEventExpressionBuilder) new EventTypeExpressionBuilder()
      {
        EventType = this.GetTypeFromTypeName(source, "EventType"),
        ExpressionType = convertResult.ConvertedValue
      });
    }

    protected virtual Guid GetConditionOperator(ItemModel source) => this.GetGuidValue(source, "ConditionOperator");
  }
}
