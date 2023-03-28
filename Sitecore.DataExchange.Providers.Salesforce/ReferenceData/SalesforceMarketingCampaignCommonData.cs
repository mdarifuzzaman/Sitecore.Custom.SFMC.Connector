// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReferenceData.SalesforceMarketingCampaignCommonData
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System;

namespace Sitecore.DataExchange.Providers.Salesforce.ReferenceData
{
  [Serializable]
  public class SalesforceMarketingCampaignCommonData
  {
    public string Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; }
  }
}
