// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers.IdentifiedContactReferenceValueReaderConverter
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
  [SupportedIds(new string[] {"{F2A8BDF9-8B55-4532-8A55-A2FA76A16EF9}"})]
  public class IdentifiedContactReferenceValueReaderConverter : BaseItemModelConverter<IValueReader>
  {
    public const string FieldNameIdentifierSource = "IdentifierSource";

    public IdentifiedContactReferenceValueReaderConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueReader> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IValueReader) new IdentifiedContactReferenceValueReader(this.GetStringValue(source, "IdentifierSource")));
    }
  }
}
