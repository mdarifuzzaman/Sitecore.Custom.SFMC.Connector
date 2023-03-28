// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Query.StringConditionExpressionConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Query
{
  [SupportedIds(new string[] {"{051888ED-A26D-452B-BB62-CB219319D488}"})]
  public class StringConditionExpressionConverter : BaseConditionExpressionConverter
  {
    public const string FieldNameValue = "Value";
    public const string FieldNameUseNullObjectForValue = "UseNullObjectForValue";
    public const string FieldNameUseEmptyStringForValue = "UseEmptyStringForValue";

    public StringConditionExpressionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override Func<IValueAccessor, object, object[]> GetDelegateForGetValues(
      ItemModel source)
    {
      string[] values = this.GetValues(source);
      if (values.Length != 0)
      {
        for (int index = 0; index < values.Length; ++index)
          values[index] = this.ConvertValueForOperator(values[index], source, "ConditionOperator");
        return (Func<IValueAccessor, object, object[]>) ((va, obj) => (object[]) values);
      }
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
              (object) this.ConvertValueForOperator(readResult.ReadValue != null ? readResult.ReadValue.ToString() : (string) null, source, "ConditionOperator")
            };
          });
      }
      return (Func<IValueAccessor, object, object[]>) ((va, obj) => new object[0]);
    }

    protected virtual string ConvertValueForOperator(
      string value,
      ItemModel source,
      string operatorFieldName)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        value = string.Empty;
      }
      else
      {
        value = this.Sanitize(value);
        Guid guidValue = this.GetGuidValue(source, operatorFieldName);
        if (guidValue == Sitecore.DataExchange.ItemIDs.StringConditionOperatorsStartsWith)
          value += "%";
        else if (guidValue == Sitecore.DataExchange.ItemIDs.StringConditionOperatorsEndsWith)
          value = "%" + value;
        else if (guidValue == Sitecore.DataExchange.ItemIDs.StringConditionOperatorsContains)
          value = "%" + value + "%";
      }
      return "'" + value + "'";
    }

    protected virtual string Sanitize(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return value;
      value = value.Replace("'", "'");
      return value;
    }

    protected virtual string[] GetValues(ItemModel source)
    {
      List<string> stringList = new List<string>();
      string stringValue = this.GetStringValue(source, "Value");
      if (!string.IsNullOrWhiteSpace(stringValue))
      {
        stringList.Add(stringValue);
      }
      else
      {
        if (this.GetBoolValue(source, "UseNullObjectForValue"))
          stringList.Add((string) null);
        if (this.GetBoolValue(source, "UseEmptyStringForValue"))
          stringList.Add(string.Empty);
      }
      return stringList.ToArray();
    }
  }
}
