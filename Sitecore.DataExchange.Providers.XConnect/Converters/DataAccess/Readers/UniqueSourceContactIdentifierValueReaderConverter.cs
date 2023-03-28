// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers.UniqueSourceContactIdentifierValueReaderConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.Readers
{
  [SupportedIds(new string[] {"{C0EE6F8E-594C-420F-82C7-15ED7229FBC8}"})]
  public class UniqueSourceContactIdentifierValueReaderConverter : 
    BaseItemModelConverter<IValueReader>
  {
    public const string FieldNameIdentifierSource = "IdentifierSource";
    public const string FieldNameIdentifierType = "IdentifierType";

    public UniqueSourceContactIdentifierValueReaderConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IValueReader> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IValueReader) new ContactIdentifierValueReader(this.GetStringValue(source, "IdentifierSource"), this.GetEnumValue<ContactIdentifierType>(source, "IdentifierType")));
    }
  }
}
