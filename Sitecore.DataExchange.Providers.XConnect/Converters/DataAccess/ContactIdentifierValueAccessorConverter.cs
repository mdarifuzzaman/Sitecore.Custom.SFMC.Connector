// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.ContactIdentifierValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{EFE72094-363D-490C-8B40-25E8531F58E1}"})]
  public class ContactIdentifierValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameConstructorMappingSet = "ConstructorMappingSet";

    public ContactIdentifierValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source) => base.GetValueWriter(source) ?? (IValueWriter) new ContactIdentifierByConstructorValueWriter(this.ConvertReferenceToModel<IMappingSet>(source, "ConstructorMappingSet"));
  }
}
