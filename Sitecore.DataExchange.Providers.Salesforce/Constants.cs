// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Constants
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

namespace Sitecore.DataExchange.Providers.Salesforce
{
  public static class Constants
  {
    public static class PipelinesBatches
    {
      public const string SalesforceCampaignsToxConnectSync = "Salesforce Campaigns to xConnect Sync";
      public const string SalesforceContactsToxConnectSync = "Salesforce Contacts to xConnect Sync";
      public const string SalesforceTasksToxConnectSync = "Salesforce Tasks to xConnect Sync";
      public const string XConnectContactsToSalesforceSync = "xConnect Contacts to Salesforce Sync";
    }

    public static class PipelinesSteps
    {
      public const string ReadCampaignsFromSalesforce = "Read Campaigns from Salesforce";
      public const string ReadContactsFromSalesforcePipeline = "Read Contacts from Salesforce Pipeline";
      public const string ReadTasksFromSalesforcePipeline = "Read Tasks from Salesforce Pipeline";
      public const string ReadContactsFromXConnectPipeline = "Read Contacts from xConnect Pipeline";
      public const string ReadSalesforceCampaigns = "Read Salesforce Campaigns";
      public const string ReadContactsFromSalesforce = "Read Contacts from Salesforce";
      public const string ReadTasksFromSalesforce = "Read Tasks from Salesforce";
      public const string SubmitRemainingContactsInSalesforceObjectQueue = "Submit Remaining Contacts In Salesforce Object Queue";
    }
  }
}
