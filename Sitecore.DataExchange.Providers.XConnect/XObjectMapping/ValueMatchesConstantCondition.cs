// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.ValueMatchesConstantCondition
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public class ValueMatchesConstantCondition : BaseXObjectMappingCondition
  {
    public object ConstantValue { get; set; }

    public override bool DoesConditionPass(object source)
    {
      ReadResult readResult = this.ReadValueFromSourceObject(source);
      return readResult.WasValueRead && object.Equals(this.ConstantValue, readResult.ReadValue);
    }
  }
}
