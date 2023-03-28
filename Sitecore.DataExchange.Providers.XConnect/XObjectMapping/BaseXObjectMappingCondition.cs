// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.BaseXObjectMappingCondition
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public abstract class BaseXObjectMappingCondition : IXObjectMappingCondition
  {
    public IValueAccessor SourceValueAccessor { get; set; }

    public abstract bool DoesConditionPass(object source);

    protected virtual ReadResult ReadValueFromSourceObject(object source) => source == null || this.SourceValueAccessor == null || this.SourceValueAccessor.ValueReader == null ? ReadResult.NegativeResult(DateTime.Now) : this.SourceValueAccessor.ValueReader.Read(source, new DataAccessContext());
  }
}
