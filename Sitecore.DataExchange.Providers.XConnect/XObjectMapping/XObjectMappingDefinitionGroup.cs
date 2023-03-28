// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.XObjectMappingDefinitionGroup
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public class XObjectMappingDefinitionGroup
  {
    public XObjectMappingDefinitionGroup() => this.EventMappingDefinitions = (ICollection<XObjectMappingDefinition>) new List<XObjectMappingDefinition>();

    public ICollection<XObjectMappingDefinition> EventMappingDefinitions { get; private set; }

    public virtual XObjectMappingDefinition GetFirstMatchingEventMappingDefinition(
      object obj)
    {
      foreach (XObjectMappingDefinition mappingDefinition in (IEnumerable<XObjectMappingDefinition>) this.EventMappingDefinitions)
      {
        if (mappingDefinition.MappingConditionSet != null && mappingDefinition.MappingConditionSet.DoConditionsPass(obj))
          return mappingDefinition;
      }
      return (XObjectMappingDefinition) null;
    }
  }
}
