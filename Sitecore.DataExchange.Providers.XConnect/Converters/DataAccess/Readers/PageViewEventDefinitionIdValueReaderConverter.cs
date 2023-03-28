// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers.PageViewEventDefinitionIdValueReaderConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers
{
  [SupportedIds(new string[] {"{D72988C8-BC3B-4A5C-AB20-D49E15DA7543}"})]
  public class PageViewEventDefinitionIdValueReaderConverter : BaseItemModelConverter<IValueReader>
  {
    private static PageViewEventDefinitionIdValueReader _reader = new PageViewEventDefinitionIdValueReader();

    public PageViewEventDefinitionIdValueReaderConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueReader> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IValueReader) PageViewEventDefinitionIdValueReaderConverter._reader);
    }
  }
}
