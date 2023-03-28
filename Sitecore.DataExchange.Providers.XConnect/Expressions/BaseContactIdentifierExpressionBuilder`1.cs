// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.BaseContactIdentifierExpressionBuilder`1
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public abstract class BaseContactIdentifierExpressionBuilder<TValue> : IEntityExpressionBuilder
  {
    public ExpressionType ExpressionType { get; set; }

    public virtual Expression<Func<TEntity, bool>> Build<TEntity>(
      ExpressionContext expressionContext)
      where TEntity : Entity
    {
      Expression<Func<Contact, bool>> expression = this.BuildContactExpression(expressionContext);
      return Expression.Lambda<Func<TEntity, bool>>(expression.Body, (IEnumerable<ParameterExpression>) expression.Parameters);
    }

    protected abstract Expression<Func<Contact, bool>> BuildContactExpression(
      ExpressionContext expressionContext);

    protected abstract TValue GetValue(ExpressionContext expressionContext);
  }
}
