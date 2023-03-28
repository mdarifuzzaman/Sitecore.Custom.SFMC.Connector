// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.UniqueSourceContactIdentifierValueAccessorConverter
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
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{2322A046-C175-4782-8DFA-7AFCA73A4796}"})]
  public class UniqueSourceContactIdentifierValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameIdentifierSource = "IdentifierSource";
    public const string FieldNameIdentifierType = "IdentifierType";
    public const string FieldNameDoNotOverwriteExistingValue = "DoNotOverwriteExistingValue";

    public UniqueSourceContactIdentifierValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueReader GetValueReader(ItemModel source) => base.GetValueReader(source) ?? (IValueReader) new ContactIdentifierValueReader(this.GetStringValue(source, "IdentifierSource"), this.GetEnumValue<ContactIdentifierType>(source, "IdentifierType"));

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      IValueWriter valueWriter = base.GetValueWriter(source);
      if (valueWriter != null)
        return valueWriter;
      string stringValue = this.GetStringValue(source, "IdentifierSource");
      bool boolValue = this.GetBoolValue(source, "DoNotOverwriteExistingValue");
      ContactIdentifierType enumValue = this.GetEnumValue<ContactIdentifierType>(source, "IdentifierType");
      return (IValueWriter) new ContactIdentifierValueWriter(stringValue, enumValue, !boolValue);
    }
  }
}
