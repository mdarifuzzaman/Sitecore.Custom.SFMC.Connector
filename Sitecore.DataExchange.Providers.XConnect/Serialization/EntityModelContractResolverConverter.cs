// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Serialization.EntityModelContractResolverConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json.Serialization;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.Serialization;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Serialization
{
  public class EntityModelContractResolverConverter : BaseItemModelConverter<IContractResolver>
  {
    public const string FieldNameSerializationRules = "SerializationRules";

    public EntityModelContractResolverConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IContractResolver> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult((IContractResolver) new EntityModelContractResolver((ICollection<IPropertySerializationRule>) this.GetSerializePropertyRules(source).ToList<IPropertySerializationRule>()));
    }

    protected IEnumerable<IPropertySerializationRule> GetSerializePropertyRules(
      ItemModel source)
    {
      return this.ConvertReferencesToModels<IPropertySerializationRule>(source, "SerializationRules");
    }
  }
}
