// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EventsFromInteractionValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{1EF83476-CEFC-435E-B3AD-F91E34A3F7F9}"})]
  public class EventsFromInteractionValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameFilter = "Filter";

    public EventsFromInteractionValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueReader GetValueReader(ItemModel source) => base.GetValueReader(source) ?? (IValueReader) new EventsFromInteractionValueReader(this.ConvertReferenceToModel<IEventExpressionBuilder>(source, "Filter"));
  }
}
