// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Expressions.ContactIdentifierTypeValueExpressionBuilder
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.XConnect;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Expressions
{
  public class ContactIdentifierTypeValueExpressionBuilder : 
    BaseContactIdentifierExpressionBuilder<ContactIdentifierType>
  {
    public IValueAccessor RightValueAccessor { get; set; }

    public ContactIdentifierType ContactIdentifierType { get; set; }

    protected override Expression<Func<Contact, bool>> BuildContactExpression(
      ExpressionContext expressionContext)
    {
      ContactIdentifierType contactIdentifierType = this.GetValue(expressionContext);
      switch (this.ExpressionType)
      {
        case ExpressionType.Equal:
          return (Expression<Func<Contact, bool>>) (c => c.Identifiers.Any<ContactIdentifier>((Func<ContactIdentifier, bool>) (i => (int) i.IdentifierType == (int) contactIdentifierType)));
        case ExpressionType.NotEqual:
          return (Expression<Func<Contact, bool>>) (c => c.Identifiers.Any<ContactIdentifier>((Func<ContactIdentifier, bool>) (i => (int) i.IdentifierType != (int) contactIdentifierType)));
        default:
          throw new NotSupportedException(string.Format("Condition operatot is not supported. (Condition operator: {0})", (object) this.ExpressionType));
      }
    }

    protected override ContactIdentifierType GetValue(
      ExpressionContext expressionContext)
    {
      if (this.RightValueAccessor?.ValueReader != null)
      {
        ReadResult readResult = this.RightValueAccessor.ValueReader.Read((object) expressionContext, new DataAccessContext());
        if (readResult.WasValueRead)
          return readResult.ReadValue as ContactIdentifierType? ?? ContactIdentifierType.Anonymous;
      }
      return this.ContactIdentifierType;
    }
  }
}
