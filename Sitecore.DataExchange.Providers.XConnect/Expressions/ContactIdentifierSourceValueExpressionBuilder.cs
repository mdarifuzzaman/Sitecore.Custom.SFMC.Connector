// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.ContactIdentifierSourceValueExpressionBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class ContactIdentifierSourceValueExpressionBuilder : 
    ContactIdentifierValueExpressionBuilder
  {
    protected override Expression<Func<Contact, bool>> BuildContactExpression(
      ExpressionContext expressionContext)
    {
      string identifierValue = this.GetValue(expressionContext);
      switch (this.ExpressionType)
      {
        case ExpressionType.Equal:
          return (Expression<Func<Contact, bool>>) (c => c.Identifiers.Any<ContactIdentifier>((Func<ContactIdentifier, bool>) (i => i.Source == identifierValue)));
        case ExpressionType.NotEqual:
          return (Expression<Func<Contact, bool>>) (c => c.Identifiers.Any<ContactIdentifier>((Func<ContactIdentifier, bool>) (i => i.Source != identifierValue)));
        default:
          throw new NotSupportedException(string.Format("Condition operatot is not supported. (Condition operator: {0})", (object) this.ExpressionType));
      }
    }
  }
}
