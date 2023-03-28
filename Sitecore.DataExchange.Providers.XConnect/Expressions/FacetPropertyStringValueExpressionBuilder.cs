// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.FacetPropertyStringValueExpressionBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class FacetPropertyStringValueExpressionBuilder : BaseFacetPropertyValueBuilder
  {
    public FacetPropertyStringValueExpressionBuilder(
      ExpressionType expressionType,
      Type facetType,
      string facetKeyName,
      string facetPropertyName,
      IValueAccessor rightValueAccessor,
      string value,
      bool shouldBeNullObjectValue,
      bool shouldBeEmptyStringValue)
      : base(expressionType, facetType, facetKeyName, facetPropertyName)
    {
      this.RightValueAccessor = rightValueAccessor;
      this.Value = value;
      this.ShouldBeNullObjectValue = shouldBeNullObjectValue;
      this.ShouldBeEmptyStringValue = shouldBeEmptyStringValue;
    }

    public IValueAccessor RightValueAccessor { get; protected set; }

    public string Value { get; protected set; }

    public bool ShouldBeNullObjectValue { get; protected set; }

    public bool ShouldBeEmptyStringValue { get; protected set; }

    public override Expression<Func<TEntity, bool>> Build<TEntity>(
      ExpressionContext expressionContext)
    {
            var value = ResolveValue(expressionContext);

            Condition.Requires(expressionContext).IsNotNull();

            var pe = Expression.Parameter(typeof(TEntity), "entity");
            var getFacetMethod = typeof(TEntity).GetMethod(nameof(Entity.GetFacet), new Type[]
            {
                typeof(string)
            }).MakeGenericMethod(FacetType);
            var getFacetExpression = Expression.Call(pe, getFacetMethod, Expression.Constant(FacetKeyName));

            var left = GetMemberExpression(getFacetExpression, FacetPropertyName);

            var right = Expression.Constant(value);
            var facetPropertyValueEqualExpression = Expression.MakeBinary(ExpressionType, left, right);

            var exp1 = Expression.Lambda<Func<TEntity, bool>>(facetPropertyValueEqualExpression, pe);
            return exp1;
        }

    public virtual string ResolveValue(ExpressionContext expressionContext)
    {
      if (this.ShouldBeEmptyStringValue)
        return string.Empty;
      if (this.ShouldBeNullObjectValue)
        return (string) null;
      if (!string.IsNullOrEmpty(this.Value))
        return this.Value;
      if (this.RightValueAccessor?.ValueReader != null)
      {
        ReadResult readResult = this.RightValueAccessor.ValueReader.Read((object) expressionContext, new DataAccessContext());
        if (readResult != null)
          return readResult.WasValueRead ? readResult.ReadValue.ToString() : throw new NotSupportedException(string.Format("Cannot read value. (value accessor: {0}; value reader: {1})", (object) this.RightValueAccessor.GetType(), (object) this.RightValueAccessor.ValueReader.GetType()));
      }
      throw new NotSupportedException("Cannot resolve value.");
    }
  }
}
