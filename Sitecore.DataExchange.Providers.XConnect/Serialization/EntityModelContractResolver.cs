// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Serialization.EntityModelContractResolver
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore.DataExchange.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.Serialization
{
  public class EntityModelContractResolver : DefaultContractResolver
  {
    public EntityModelContractResolver()
    {
    }

    public EntityModelContractResolver(
      ICollection<IPropertySerializationRule> serializePropertyRules)
    {
      this.SerializePropertyRules = serializePropertyRules;
    }

    public ICollection<IPropertySerializationRule> SerializePropertyRules { get; protected set; }

    protected override JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
      if (this.SerializePropertyRules == null || !this.SerializePropertyRules.Any<IPropertySerializationRule>())
        return jsonProperty;
      bool shouldNotSerialize = this.SerializePropertyRules.Any<IPropertySerializationRule>((Func<IPropertySerializationRule, bool>) (hp => !hp.ShouldSerializeProperty(jsonProperty)));
      jsonProperty.ShouldSerialize = (Predicate<object>) (instance => !shouldNotSerialize);
      return jsonProperty;
    }
  }
}
