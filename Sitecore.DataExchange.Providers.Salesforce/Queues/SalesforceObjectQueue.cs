// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.SalesforceObjectQueue
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Salesforce.Common.Models.Xml;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  public class SalesforceObjectQueue
  {
    public SalesforceObjectQueue()
    {
      this.InsertList = new SObjectList<SObject>();
      this.UpdateList = new SObjectList<SObject>();
    }

    public SObjectList<SObject> InsertList { get; set; }

    public SObjectList<SObject> UpdateList { get; set; }
  }
}
