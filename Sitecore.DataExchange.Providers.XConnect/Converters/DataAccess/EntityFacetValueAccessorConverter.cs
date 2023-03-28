// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.EntityFacetValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect.Schema;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{40C62E1A-4985-49BC-BBE6-AEF11228151A}"})]
  public class EntityFacetValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameMappingSet = "MappingSet";
    public const string FieldNameFacetDefinition = "FacetDefinition";

    public EntityFacetValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

        protected override IValueWriter GetValueWriter(ItemModel source)
        {
            var valueWriter = base.GetValueWriter(source);
            if (valueWriter  == null) 
                return (IValueWriter)new FacetMapValueWriter(this.ConvertReferenceToModel<IMappingSet>(source, "MappingSet"), this.ConvertReferenceToModel<XdbFacetDefinition>(source, "FacetDefinition"));
            return valueWriter;
        }
  }
}
