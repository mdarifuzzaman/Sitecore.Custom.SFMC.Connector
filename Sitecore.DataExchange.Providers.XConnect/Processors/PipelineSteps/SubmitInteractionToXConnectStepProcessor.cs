// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.SubmitInteractionToXConnectStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class SubmitInteractionToXConnectStepProcessor : 
    BaseSubmitEntityToXConnectStepProcessor<InteractionModel, Interaction>
  {
    protected override Interaction ConvertEntityModelToEntity(
      InteractionModel entityModel,
      IXConnectClientHelper clientHelper,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return clientHelper.ToInteraction(entityModel);
    }

    protected override IEnumerable<IXdbOperation> AddEntityToBatch(
      InteractionModel entityModel,
      Interaction entity,
      IXdbContext client,
      IXConnectClientHelper helper,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      List<IXdbOperation> batch = new List<IXdbOperation>();
      if (entity != null)
      {
        PageViewEvent pageViewEvent1 = new PageViewEvent(DateTime.UtcNow, Guid.NewGuid(), 3, "en");
        pageViewEvent1.Duration = new TimeSpan(0, 0, 30);
        PageViewEvent pageViewEvent2 = pageViewEvent1;
        entity.Events.Add((Event) pageViewEvent2);
        AddInteractionOperation interactionOperation = client.AddInteraction(entity);
        batch.Add((IXdbOperation) interactionOperation);
        foreach (KeyValuePair<string, Facet> facet in (IEnumerable<KeyValuePair<string, Facet>>) entityModel.Facets)
        {
          SetFacetOperation<Facet> setFacetOperation = client.SetFacet<Facet>((IEntityReference) entity, facet.Value);
          batch.Add((IXdbOperation) setFacetOperation);
        }
      }
      return (IEnumerable<IXdbOperation>) batch;
    }
  }
}
