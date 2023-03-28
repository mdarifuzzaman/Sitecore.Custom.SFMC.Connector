// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.InConditionExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  [SupportedIds(new string[] {"{C13A4E3E-3D13-4662-B5EC-FCA3046FD586}"})]
  public class InConditionExpressionConverter : BaseConditionExpressionConverter
  {
    public const string FieldNameValue = "Value";

    public InConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source)
    {
      string value = this.GetStringValue(source, "Value");
      if (!string.IsNullOrEmpty(value))
        return (Func<IValueAccessor, object, object[]>) ((va, obj) => new object[1]
        {
          (object) value
        });
      IValueAccessor rightValueAccessor = this.GetRightValueAccessor(source);
      if (rightValueAccessor != null)
      {
        IValueReader valueReader = rightValueAccessor.ValueReader;
        if (valueReader != null)
          return (Func<IValueAccessor, object, object[]>) ((va, obj) =>
          {
            ReadResult readResult = valueReader.Read(obj, new DataAccessContext());
            if (!readResult.WasValueRead)
              return new object[0];
            return new object[1]
            {
              (object) (readResult.ReadValue != null ? readResult.ReadValue.ToString() : (string) null)
            };
          });
      }
      return (Func<IValueAccessor, object, object[]>) ((va, obj) => new object[0]);
    }
  }
}
