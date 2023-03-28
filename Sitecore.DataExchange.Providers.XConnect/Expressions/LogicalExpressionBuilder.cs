// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.LogicalExpressionBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class LogicalExpressionBuilder : IEntityExpressionBuilder
  {
    public LogicalExpressionBuilder(
      ExpressionType expressionType,
      IEnumerable<IEntityExpressionBuilder> entityExpressionBuilders)
    {
      this.ExpressionType = expressionType;
      this.EntityExpressionBuilders = entityExpressionBuilders;
    }

    public ExpressionType ExpressionType { get; protected set; }

    public IEnumerable<IEntityExpressionBuilder> EntityExpressionBuilders { get; protected set; }

    public virtual Expression<Func<TEntity, bool>> Build<TEntity>(
      ExpressionContext expressionContext)
      where TEntity : Entity
    {
      if (this.EntityExpressionBuilders == null)
        return (Expression<Func<TEntity, bool>>) null;
      if (this.EntityExpressionBuilders.Count<IEntityExpressionBuilder>() == 1)
        return this.EntityExpressionBuilders.First<IEntityExpressionBuilder>().Build<TEntity>(expressionContext);
      IEntityExpressionBuilder[] array = this.EntityExpressionBuilders.ToArray<IEntityExpressionBuilder>();
      Expression<Func<TEntity, bool>> left = array[0].Build<TEntity>(expressionContext);
      Expression<Func<TEntity, bool>> right = array[1].Build<TEntity>(expressionContext);
      BinaryExpression body = Expression.MakeBinary(this.ExpressionType, (Expression) left, (Expression) right);
      List<ParameterExpression> parameters = new List<ParameterExpression>();
      parameters.AddRange((IEnumerable<ParameterExpression>) left.Parameters);
      parameters.AddRange((IEnumerable<ParameterExpression>) right.Parameters);
      return Expression.Lambda<Func<TEntity, bool>>((Expression) body, (IEnumerable<ParameterExpression>) parameters);
    }
  }
}
