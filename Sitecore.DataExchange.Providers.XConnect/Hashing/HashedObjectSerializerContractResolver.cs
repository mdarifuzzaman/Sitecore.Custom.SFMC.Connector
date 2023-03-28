// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Hashing.HashedObjectSerializerContractResolver
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Client.Serialization;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using System;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.Hashing
{
  internal class HashedObjectSerializerContractResolver : XdbJsonContractResolver
  {
    public HashedObjectSerializerContractResolver(
      XdbModel model,
      bool serializeFacets = true,
      bool serializeContactInteractions = true,
      bool serializeEvents = false)
      : base(model, serializeFacets, serializeContactInteractions)
    {
      this.SerializeEvents = serializeEvents;
    }

    private bool SerializeEvents { get; set; }

    protected override JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty property = base.CreateProperty(member, memberSerialization);
      if (property.DeclaringType == typeof (Event) && (property.PropertyName == "Id" || property.PropertyName == "ParentEventId"))
      {
        property.ShouldSerialize = (Predicate<object>) (instance => (PageViewEvent) instance == null);
        return property;
      }
      if (property.PropertyName == "ConcurrencyToken" || property.PropertyName == "LastModified")
      {
        property.ShouldSerialize = (Predicate<object>) (instance => false);
        return property;
      }
      if ((property.DeclaringType.IsSubclassOf(typeof (EntityModel)) || property.DeclaringType == typeof (EntityModel)) && property.PropertyName == "Entity")
        property.ShouldSerialize = (Predicate<object>) (instance => false);
      if ((property.DeclaringType.IsSubclassOf(typeof (Entity)) || property.DeclaringType == typeof (Entity)) && property.PropertyName == "Entity")
        property.ShouldSerialize = (Predicate<object>) (instance => false);
      if ((property.DeclaringType.IsSubclassOf(typeof (Facet)) || property.DeclaringType == typeof (Facet)) && !this.SerializeFacets)
        property.ShouldSerialize = (Predicate<object>) (instance => false);
      if (property.DeclaringType == typeof (Interaction) && !this.SerializeContactInteractions)
        property.ShouldSerialize = (Predicate<object>) (instance => false);
      return property;
    }
  }
}
