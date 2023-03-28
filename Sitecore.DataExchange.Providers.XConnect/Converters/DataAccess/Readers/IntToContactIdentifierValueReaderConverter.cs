// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers.IntToContactIdentifierValueReaderConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Framework.Conditions;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers
{
  [SupportedIds(new string[] {"{200CE6E1-27D8-499A-8EB1-F469DB9FE2C0}"})]
  public class IntToContactIdentifierValueReaderConverter : BaseItemModelConverter<IValueReader>
  {
    public const string FieldNameTypeMapper = "TypeMapper";

    public IntToContactIdentifierValueReaderConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueReader> ConvertSupportedItem(
      ItemModel source)
    {
      string stringValue = this.GetStringValue(source, "TypeMapper");
      if (string.IsNullOrWhiteSpace(stringValue))
        return this.PositiveResult((IValueReader) new IntToContactIdentifierTypeValueReader());
      IDictionary<int, string> typeMapper;
      return !this.TryParseMappingValue(stringValue, out typeMapper) ? this.NegativeResult(source, "Unable to parse the type mapper assigned to the item.") : this.PositiveResult((IValueReader) new IntToContactIdentifierTypeValueReader(typeMapper));
    }

    protected virtual bool TryParseMappingValue(
      string mapperValue,
      out IDictionary<int, string> typeMapper)
    {
      if (string.IsNullOrWhiteSpace(mapperValue))
      {
        typeMapper = (IDictionary<int, string>) null;
        return false;
      }
      try
      {
        typeMapper = (IDictionary<int, string>) ((IEnumerable<string>) mapperValue.Split(new char[1]
        {
          '&'
        }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string[]>((Func<string, string[]>) (part => part.Split('='))).ToDictionary<string[], int, string>((Func<string[], int>) (split => int.Parse(split[0])), (Func<string[], string>) (split => this.GetIdentifierTyepName(split[1])));
        return true;
      }
      catch (Exception ex)
      {
        typeMapper = (IDictionary<int, string>) null;
        return false;
      }
    }

    private string GetIdentifierTyepName(string id)
    {
      Condition.Requires<string>(id).IsNotNullOrWhiteSpace();
      ItemModel itemModel = this.ItemModelRepository.Get(new Guid(id.Replace("%7B", string.Empty).Replace("%7D", string.Empty)));
      Condition.Ensures<ItemModel>(itemModel).IsNotNull<ItemModel>();
      return itemModel["ItemName"].ToString();
    }
  }
}
