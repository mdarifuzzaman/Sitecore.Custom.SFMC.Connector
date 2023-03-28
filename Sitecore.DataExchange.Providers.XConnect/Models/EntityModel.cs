// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Models.EntityModel
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Models
{
  public abstract class EntityModel
  {
    [JsonIgnore]
    public Entity Entity { get; set; }

    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonIgnore]
    public string InternalDescription { get; set; }

    public IDictionary<string, Facet> Facets { get; set; } = (IDictionary<string, Facet>) new Dictionary<string, Facet>();

    public abstract Entity AddToClient(
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationsAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected virtual void AddFacets(
      IEntityReference entity,
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationsAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (this.Facets == null || this.Facets.Count == 0)
        return;
      foreach (KeyValuePair<string, Facet> facet in (IEnumerable<KeyValuePair<string, Facet>>) this.Facets)
      {
        SetFacetOperation<Facet> setFacetOperation = client.SetFacet<Facet>(entity, facet.Key, facet.Value);
        onOperationsAddedToClient((IEnumerable<IXdbOperation>) new SetFacetOperation<Facet>[1]
        {
          setFacetOperation
        }, client, minBatchSize, pipelineStep, pipelineContext, logger);
      }
    }
  }
}
