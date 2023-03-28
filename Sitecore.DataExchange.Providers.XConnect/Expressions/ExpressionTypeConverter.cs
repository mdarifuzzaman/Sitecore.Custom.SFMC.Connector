// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.ExpressionTypeConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class ExpressionTypeConverter : IConverter<Guid, ExpressionType>
  {
    public virtual ConvertResult<ExpressionType> Convert(Guid source)
    {
      if (source == ItemIDs.StringConditionOperatorsEqual || source == ItemIDs.ConditionOperatorsEqual || source == ItemIDs.ObjectConditionOperatorsEqual || source == ItemIDs.ConditionOperatorsExist)
        return ConvertResult<ExpressionType>.PositiveResult(ExpressionType.Equal);
      return source == ItemIDs.StringConditionOperatorsNotEqual || source == ItemIDs.ConditionOperatorsNotEqual || source == ItemIDs.ObjectConditionOperatorsNotEqual || source == ItemIDs.ConditionOperatorsNotExist ? ConvertResult<ExpressionType>.PositiveResult(ExpressionType.NotEqual) : ConvertResult<ExpressionType>.NegativeResult("Unsupported Expression.");
    }
  }
}
