// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.QueueOperationSettings
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  public class QueueOperationSettings : IPlugin
  {
    public string ObjectName { get; set; }

    public string SitecoreIdFieldName { get; set; } = "SitecoreId__c";
  }
}
