// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.XClrObjectPropertyValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{78D188E0-1D6B-4EEA-828C-347EA8925685}"})]
  public class XClrObjectPropertyValueAccessorConverter : XObjectPropertyValueAccessorConverter
  {
    public const string PropertyNameClrObject = "ClrObject";

    public XClrObjectPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      IValueWriter valueWriter = base.GetValueWriter(source);
      if (valueWriter != null)
        return valueWriter;
      return (IValueWriter) new TargetReaderValueWriter()
      {
        ValueAccessor = (IValueAccessor) new ValueAccessor()
        {
          ValueReader = (IValueReader) new SequentialValueReader()
          {
            Readers = {
              (IValueReader) new PropertyValueReader("ClrObject")
            }
          },
          ValueWriter = (IValueWriter) new PropertyValueWriter(this.GetStringValue(source, "PropertyName"))
        }
      };
    }
  }
}
