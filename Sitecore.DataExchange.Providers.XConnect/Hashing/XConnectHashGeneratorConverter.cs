// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Hashing.XConnectHashGeneratorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Hashing;
using Sitecore.DataExchange.Providers.XConnect.Serialization;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Hashing
{
  public class XConnectHashGeneratorConverter : BaseItemModelConverter<IHashGenerator>
  {
    public const string FieldNameContractResolver = "ContractResolver";

    public XConnectHashGeneratorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<IHashGenerator> ConvertSupportedItem(
      ItemModel source)
    {
      IContractResolver contactResolver = this.GetContactResolver(source);
      return this.PositiveResult((IHashGenerator) new HashGenerator(new JsonSerializerSettings()
      {
        ContractResolver = contactResolver,
        Converters = (IList<JsonConverter>) new List<JsonConverter>()
        {
          (JsonConverter) new StringEnumConverter(),
          (JsonConverter) new ContactIdentifiersConverter()
        },
        Formatting = Formatting.None
      }));
    }

    protected virtual IContractResolver GetContactResolver(ItemModel source) => this.ConvertReferenceToModel<IContractResolver>(source, "ContractResolver") ?? (IContractResolver) new EntityModelContractResolver();
  }
}
