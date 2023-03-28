// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.ParameterPositionValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{6E06A3D0-CA23-4D35-BECF-C3E283E2F506}"})]
  public class ParameterPositionValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameParameterPosition = "ParameterPosition";

    public ParameterPositionValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueReader GetValueReader(ItemModel source) => base.GetValueReader(source) ?? (IValueReader) new DictionaryValueReader<int, object>(this.GetIntValue(source, "ParameterPosition"));

    protected override IValueWriter GetValueWriter(ItemModel source) => base.GetValueWriter(source) ?? (IValueWriter) new DictionaryValueWriter<int, object>(this.GetIntValue(source, "ParameterPosition"));
  }
}
