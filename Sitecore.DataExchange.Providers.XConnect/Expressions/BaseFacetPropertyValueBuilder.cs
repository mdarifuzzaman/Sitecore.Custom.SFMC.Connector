// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.BaseFacetPropertyValueBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public abstract class BaseFacetPropertyValueBuilder : IEntityExpressionBuilder
  {
    protected BaseFacetPropertyValueBuilder(
      ExpressionType expressionType,
      Type facetType,
      string facetKeyName,
      string facetPropertyName)
    {
      Condition.Ensures<Type>(facetType).IsNotNull<Type>();
      Condition.Ensures<string>(facetKeyName).IsNotNullOrWhiteSpace();
      Condition.Ensures<string>(facetPropertyName).IsNotNullOrWhiteSpace();
      this.ExpressionType = expressionType;
      this.FacetType = facetType;
      this.FacetKeyName = facetKeyName;
      this.FacetPropertyName = facetPropertyName;
    }

    public ExpressionType ExpressionType { get; protected set; }

    public Type FacetType { get; protected set; }

    public string FacetKeyName { get; protected set; }

    public string FacetPropertyName { get; protected set; }

    public MemberExpression GetMemberExpression(
      Expression param,
      string propertyName)
    {
      if (!propertyName.Contains("."))
        return Expression.Property(param, propertyName);
      int length = propertyName.IndexOf(".");
      return this.GetMemberExpression((Expression) Expression.Property(param, propertyName.Substring(0, length)), propertyName.Substring(length + 1));
    }

    public abstract Expression<Func<TEntity, bool>> Build<TEntity>(
      ExpressionContext expressionContext)
      where TEntity : Entity;
  }
}
