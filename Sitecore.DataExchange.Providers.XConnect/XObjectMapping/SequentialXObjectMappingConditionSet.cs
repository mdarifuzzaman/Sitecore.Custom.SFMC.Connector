// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.SequentialXObjectMappingConditionSet
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public class SequentialXObjectMappingConditionSet : IXObjectMappingConditionSet
  {
    public SequentialXObjectMappingConditionSet()
    {
      this.MappingIncludeConditions = (ICollection<IXObjectMappingCondition>) new List<IXObjectMappingCondition>();
      this.MappingExcludeConditions = (ICollection<IXObjectMappingCondition>) new List<IXObjectMappingCondition>();
    }

    public ICollection<IXObjectMappingCondition> MappingIncludeConditions { get; private set; }

    public ICollection<IXObjectMappingCondition> MappingExcludeConditions { get; private set; }

    public virtual bool DoConditionsPass(object source)
    {
      foreach (IXObjectMappingCondition includeCondition in (IEnumerable<IXObjectMappingCondition>) this.MappingIncludeConditions)
      {
        if (!includeCondition.DoesConditionPass(source))
          return false;
      }
      foreach (IXObjectMappingCondition excludeCondition in (IEnumerable<IXObjectMappingCondition>) this.MappingExcludeConditions)
      {
        if (excludeCondition.DoesConditionPass(source))
          return false;
      }
      return true;
    }
  }
}
