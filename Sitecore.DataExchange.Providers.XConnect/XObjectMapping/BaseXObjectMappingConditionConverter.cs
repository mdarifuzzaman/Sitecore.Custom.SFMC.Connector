// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.BaseXObjectMappingConditionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public abstract class BaseXObjectMappingConditionConverter : 
    BaseItemModelConverter<IXObjectMappingCondition>
  {
    public const string SourceValueAccessor = "SourceValueAccessor";

    protected BaseXObjectMappingConditionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected virtual IValueAccessor GetSourceValueAccessor(ItemModel source) => this.ConvertReferenceToModel<IValueAccessor>(source, "SourceValueAccessor");
  }
}
