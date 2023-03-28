// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.ContactIdentifierSourceValueExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class ContactIdentifierSourceValueExpressionConverter : 
    ContactIdentifierValueExpressionConverter
  {
    public ContactIdentifierSourceValueExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IEntityExpressionBuilder> ConvertSupportedItem(
      ItemModel source)
    {
      ConvertResult<ExpressionType> convertResult = this.ExpressionTypeConverter.Convert(this.GetConditionOperator(source));
      if (!convertResult.WasConverted)
        return this.NegativeResult(source, "Cannot convert condition operator to expression type. (Reason: " + convertResult.Reason + ")");
      ExpressionType convertedValue = convertResult.ConvertedValue;
      IValueAccessor rightValueAccessor = this.GetRightValueAccessor(source);
      bool flag1 = this.ShouldUseEmptyStringForValue(source);
      bool flag2 = this.ShouldUseNullForValue(source);
      string str = this.GetValue(source);
      ContactIdentifierSourceValueExpressionBuilder expressionBuilder = new ContactIdentifierSourceValueExpressionBuilder();
      expressionBuilder.ExpressionType = convertedValue;
      expressionBuilder.RightValueAccessor = rightValueAccessor;
      expressionBuilder.Value = str;
      expressionBuilder.UseEmptyStringForValue = flag1;
      expressionBuilder.UseNullObjectForValue = flag2;
      return this.PositiveResult((IEntityExpressionBuilder) expressionBuilder);
    }
  }
}
