// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.XObjectListPropertyValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{316E7B8C-99F3-4B7A-A1C2-7B30E15AAF1C}"})]
  public class XObjectListPropertyValueAccessorConverter : XObjectPropertyValueAccessorConverter
  {
    public const string FieldNameMappingSet = "MappingSet";
    public const string FieldNameCreateNewListIfPropertyIsNull = "CreateNewListIfPropertyIsNull";

    public XObjectListPropertyValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source) => base.GetValueWriter(source) ?? (IValueWriter) new XObjectListPropertyValueWriter(this.GetStringValue(source, "PropertyName"), this.ConvertReferenceToModel<IMappingSet>(source, "MappingSet"), this.GetBoolValue(source, "CreateNewListIfPropertyIsNull"));
  }
}
