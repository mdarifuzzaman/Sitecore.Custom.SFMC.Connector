// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.EntityFacetPropertyValueExpressionBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class EntityFacetPropertyValueExpressionBuilder : IEntityExpressionBuilder
  {
    public EntityFacetPropertyValueExpressionBuilder(Type facetType, string propertyFacetName)
      : this(facetType, propertyFacetName, ExpressionType.Equal)
    {
    }

    public EntityFacetPropertyValueExpressionBuilder(
      Type facetType,
      string propertyFacetName,
      ExpressionType expressionType)
    {
      Condition.Requires<Type>(facetType).IsNotNull<Type>();
      Condition.Requires<string>(propertyFacetName).IsNotNullOrWhiteSpace();
      this.FacetType = facetType;
      this.PropertyFacetName = propertyFacetName;
      this.ExpressionType = expressionType;
    }

    public Type FacetType { get; private set; }

    public string PropertyFacetName { get; private set; }

    public ExpressionType ExpressionType { get; private set; }

    public virtual Expression<Func<TEntity, bool>> Build<TEntity>(
      ExpressionContext expressionContext)
      where TEntity : Entity
    {
      Condition.Requires<ExpressionContext>(expressionContext).IsNotNull<ExpressionContext>();
      string propertyValue = expressionContext.GetPlugin<EntityFacetPropertySettings>().PropertyValue;
      ParameterExpression instance = Expression.Parameter(typeof (TEntity), "entity");
      MethodInfo method = typeof (TEntity).GetMethod("GetFacet", new Type[0]).MakeGenericMethod(this.FacetType);
      MethodCallExpression left = Expression.Call((Expression) instance, method);
      return Expression.Lambda<Func<TEntity, bool>>((Expression) Expression.AndAlso((Expression) Expression.NotEqual((Expression) left, (Expression) Expression.Constant((object) null)), (Expression) Expression.MakeBinary(this.ExpressionType, (Expression) Expression.Property((Expression) left, this.PropertyFacetName), (Expression) Expression.Constant((object) propertyValue))), instance);
    }
  }
}
