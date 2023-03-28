// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.DataAccess.ApplyMappingRules.TargetCollectionNonEmptyMappingRule
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using System.Collections;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.ApplyMappingRules
{
  public class TargetCollectionNonEmptyMappingRule : IApplyMappingRule
  {
    public bool ShouldMappingBeApplied(object value, IMapping mapping, MappingContext context)
    {
      ReadResult readResult = mapping.TargetAccessor.ValueReader.Read(context.Target, new DataAccessContext());
      return readResult.WasValueRead && readResult.ReadValue != null && readResult.ReadValue is ICollection && ((ICollection) readResult.ReadValue).Count > 0;
    }
  }
}
