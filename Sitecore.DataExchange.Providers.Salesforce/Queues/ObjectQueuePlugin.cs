// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.ObjectQueuePlugin
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  public class ObjectQueuePlugin : IPlugin
  {
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, SalesforceObjectQueue>> _concurrentDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<int, SalesforceObjectQueue>>();

    public int MinimumBatchSize { get; set; }

    public Endpoint Endpoint { get; set; }

    public SalesforceObjectQueue GetQueue(string objectName, int id) => this._concurrentDictionary.GetOrAdd(objectName.ToLower(), new ConcurrentDictionary<int, SalesforceObjectQueue>()).GetOrAdd(id, new SalesforceObjectQueue());

    public IDictionary<string, ConcurrentDictionary<int, SalesforceObjectQueue>> GetQueues() => (IDictionary<string, ConcurrentDictionary<int, SalesforceObjectQueue>>) this._concurrentDictionary;
  }
}
