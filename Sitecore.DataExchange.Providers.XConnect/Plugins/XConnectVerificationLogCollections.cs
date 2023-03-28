// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Plugins.XConnectVerificationLogCollections
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.VerificationLog;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Plugins
{
  public class XConnectVerificationLogCollections : IPlugin
  {
    public XConnectVerificationLogCollections()
    {
      this.Entries = new Dictionary<int, List<IVerificationLogEntry>>();
      this.Entries[0] = new List<IVerificationLogEntry>();
    }

    internal Dictionary<int, List<IVerificationLogEntry>> Entries { get; set; }

    public List<IVerificationLogEntry> GetCollectionForCurrentThread(
      PipelineContext pipelineContext)
    {
      int currentThreadId = pipelineContext.GetCurrentThreadId();
      if (!this.Entries.ContainsKey(currentThreadId))
        this.Entries[currentThreadId] = new List<IVerificationLogEntry>();
      return this.Entries[currentThreadId];
    }
  }
}
