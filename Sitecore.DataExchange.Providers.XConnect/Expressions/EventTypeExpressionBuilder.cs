// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.EventTypeExpressionBuilder
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
    public class EventTypeExpressionBuilder : IEventExpressionBuilder
    {
        public ExpressionType ExpressionType { get; set; }

        public Type EventType { get; set; }

        public virtual Expression<Func<Event, bool>> Build(
          ExpressionContext expressionContext)
        {
            Condition.Requires(EventType).IsNotNull();

            if (ExpressionType == ExpressionType.Equal)
            {
                return e => e.GetType() == EventType;
            }

            if (ExpressionType == ExpressionType.NotEqual)
            {
                return e => e.GetType() != EventType;
            }

            throw new NotSupportedException($"Not supported expression type. (expression type: {ExpressionType})");
        }
    }
}
