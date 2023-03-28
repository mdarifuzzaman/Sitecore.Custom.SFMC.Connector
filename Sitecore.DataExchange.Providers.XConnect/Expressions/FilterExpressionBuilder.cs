// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.FilterExpressionBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Extensions;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class FilterExpressionBuilder : IEntityExpressionBuilder
  {
    public string LogicalOperator { get; set; }

    public ICollection<IEntityExpressionBuilder> ConditionExpressionBuilders { get; private set; } = (ICollection<IEntityExpressionBuilder>) new List<IEntityExpressionBuilder>();

    public virtual List<Expression<Func<TEntity, bool>>> ToExpressionList<TEntity>() where TEntity : Entity
    {
      List<Expression<Func<TEntity, bool>>> expressionList = new List<Expression<Func<TEntity, bool>>>();
      ExpressionContext expressionContext = new ExpressionContext();
      foreach (IEntityExpressionBuilder expressionBuilder in (IEnumerable<IEntityExpressionBuilder>) this.ConditionExpressionBuilders)
        expressionList.Add(expressionBuilder.Build<TEntity>(expressionContext));
      return expressionList.Count == 0 ? (List<Expression<Func<TEntity, bool>>>) null : expressionList;
    }

    public Expression<Func<TEntity, bool>> Build<TEntity>(
      ExpressionContext expressionContext)
      where TEntity : Entity
    {
      List<Expression<Func<TEntity, bool>>> expressionList = this.ToExpressionList<TEntity>();
      string str = this.LogicalOperator.ToLower().Trim();
      if (expressionList == null || expressionList.Count == 0)
        return (Expression<Func<TEntity, bool>>) null;
      if (expressionList.Count == 1)
        return expressionList.First<Expression<Func<TEntity, bool>>>();
      Expression body = expressionList[0].Body;
      Expression right1 = expressionList[1].Body.ReplaceParameter(expressionList[1].Parameters[0], (Expression) expressionList[0].Parameters[0]);
      BinaryExpression binaryExpression = !(str == "or") ? Expression.AndAlso(body, right1) : Expression.OrElse(body, right1);
      for (int index = 2; index < expressionList.Count; ++index)
      {
        Expression right2 = expressionList[index].Body.ReplaceParameter(expressionList[index].Parameters[0], (Expression) expressionList[0].Parameters[0]);
        binaryExpression = !(str == "or") ? Expression.AndAlso((Expression) binaryExpression, right2) : Expression.OrElse((Expression) binaryExpression, right2);
      }
      return Expression.Lambda<Func<TEntity, bool>>((Expression) binaryExpression, expressionList[0].Parameters[0]);
    }
  }
}
