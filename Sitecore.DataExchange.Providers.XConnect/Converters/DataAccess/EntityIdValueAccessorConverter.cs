// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EntityIdValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{CD0BD3FC-9A38-4EC4-A2D8-5A97FABA5F41}"})]
  public class EntityIdValueAccessorConverter : ValueAccessorConverter
  {
    public EntityIdValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueAccessor> ConvertSupportedItem(
      ItemModel source)
    {
      ConvertResult<IValueAccessor> convertResult = base.ConvertSupportedItem(source);
      if (!convertResult.WasConverted)
        return convertResult;
      IValueAccessor convertedValue = convertResult.ConvertedValue;
      if (convertedValue.ValueReader == null)
        convertedValue.ValueReader = (IValueReader) new EntityIdValueReader();
      if (convertedValue.ValueWriter == null)
        convertedValue.ValueWriter = (IValueWriter) new EntityIdValueWriter();
      return this.PositiveResult(convertedValue);
    }
  }
}
