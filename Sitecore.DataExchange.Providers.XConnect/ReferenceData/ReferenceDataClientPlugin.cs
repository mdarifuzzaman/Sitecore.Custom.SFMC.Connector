// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ReferenceDataClientPlugin
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Microsoft.Extensions.Logging;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Xdb.Common.Web;
using Sitecore.Xdb.ReferenceData.Client;
using Sitecore.Xdb.ReferenceData.Core;
using Sitecore.Xdb.ReferenceData.Core.Converter;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  public class ReferenceDataClientPlugin : BaseXpfClientPlugin<IReferenceDataClient>
  {
    private IDictionary<int, IList<BaseDefinition>> _batches = (IDictionary<int, IList<BaseDefinition>>) new Dictionary<int, IList<BaseDefinition>>();

    public ReferenceDataClientPlugin(Uri baseAddress, Type commonDataType, Type cultureDataType)
    {
      this.BaseAddress = baseAddress;
      this.CommonDataType = commonDataType;
      this.CultureDataType = cultureDataType;
      this.DefinitionEnvelopeConverter = (IDefinitionEnvelopeConverter) new DefinitionEnvelopeJsonConverter();
      this.ReferenceDataHelper = new ReferenceDataHelper();
    }

    public int MinimumBatchSize { get; set; }

    public Uri BaseAddress { get; private set; }

    public Type CommonDataType { get; private set; }

    public Type CultureDataType { get; private set; }

    public DefinitionTypeKey DefinitionTypeKey { get; set; }

    public short DefinitionTypeVersion { get; set; }

    public IHttpClientHandlerModifier[] Modifiers { get; set; }

    public IDefinitionEnvelopeConverter DefinitionEnvelopeConverter { get; set; }

    public ReferenceDataHelper ReferenceDataHelper { get; set; }

    public IList<BaseDefinition> GetBatch(int threadId)
    {
      if (!this._batches.ContainsKey(threadId))
        this._batches[threadId] = (IList<BaseDefinition>) new List<BaseDefinition>();
      return this._batches[threadId];
    }

    protected override IReferenceDataClient CreateNewClient(int threadId) => (IReferenceDataClient) new ReferenceDataHttpClient(this.DefinitionEnvelopeConverter, this.BaseAddress, (IEnumerable<IHttpClientHandlerModifier>) this.Modifiers, (ILogger<ReferenceDataHttpClient>) new Logger<ReferenceDataHttpClient>((ILoggerFactory) new LoggerFactory()));
  }
}
