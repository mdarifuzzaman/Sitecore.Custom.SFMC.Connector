﻿// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.IXObjectMappingConditionSet
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public interface IXObjectMappingConditionSet
  {
    ICollection<IXObjectMappingCondition> MappingIncludeConditions { get; }

    ICollection<IXObjectMappingCondition> MappingExcludeConditions { get; }

    bool DoConditionsPass(object source);
  }
}
