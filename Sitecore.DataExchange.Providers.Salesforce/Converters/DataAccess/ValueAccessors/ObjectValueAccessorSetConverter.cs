// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.ValueAccessors.ObjectValueAccessorSetConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Converters.DataAccess.ValueAccessors
{
  [SupportedIds(new string[] {"{39C08D1A-9DC9-48BB-B916-97E311905883}"})]
  public class ObjectValueAccessorSetConverter : 
    ChildBasedValueAccessorSetConverter,
    IConverter<ItemModel, ICollection<string>>
  {
    public ObjectValueAccessorSetConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    ConvertResult<ICollection<string>> IConverter<ItemModel, ICollection<string>>.Convert(
      ItemModel source)
    {
      if (!this.IsSupportedItem(source))
        return ConvertResult<ICollection<string>>.NegativeResult(this.FormatMessageForNegativeResult(source, "The source item is not supported by this converter."));
      HashSet<string> convertedValue = new HashSet<string>();
      IEnumerable<ItemModel> childItemModels = this.GetChildItemModels(source);
      if (childItemModels != null)
      {
        foreach (ItemModel itemModel in childItemModels)
        {
          if (this.GetBoolValue(itemModel, "Enabled"))
          {
            string stringValue = this.GetStringValue(itemModel, "FieldNameForSelect");
            if (string.IsNullOrWhiteSpace(stringValue))
              stringValue = this.GetStringValue(itemModel, "FieldName");
            if (!string.IsNullOrWhiteSpace(stringValue))
              convertedValue.Add(stringValue);
          }
        }
      }
      return ConvertResult<ICollection<string>>.PositiveResult((ICollection<string>) convertedValue);
    }
  }
}
