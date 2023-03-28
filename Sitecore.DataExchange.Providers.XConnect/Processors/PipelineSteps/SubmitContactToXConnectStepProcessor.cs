// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.SubmitContactToXConnectStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class SubmitContactToXConnectStepProcessor : 
    BaseSubmitEntityToXConnectStepProcessor<ContactModel, Contact>
  {
    protected override Contact ConvertEntityModelToEntity(
      ContactModel entityModel,
      IXConnectClientHelper clientHelper,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return clientHelper.ToContact(entityModel);
    }

    protected override IEnumerable<IXdbOperation> AddEntityToBatch(
      ContactModel entityModel,
      Contact entity,
      IXdbContext client,
      IXConnectClientHelper helper,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      List<IXdbOperation> batch = new List<IXdbOperation>();
      if (entityModel.Id == Guid.Empty)
      {
        AddContactOperation contactOperation = client.AddContact(entity);
        batch.Add((IXdbOperation) contactOperation);
      }
      else
      {
        foreach (KeyValuePair<string, Facet> facet in (IEnumerable<KeyValuePair<string, Facet>>) entityModel.Facets)
        {
          SetFacetOperation<Facet> setFacetOperation = client.SetFacet<Facet>(new FacetReference((IEntityReference) new ContactReference(entityModel.Id), facet.Key), facet.Value);
          batch.Add((IXdbOperation) setFacetOperation);
        }
      }
      return (IEnumerable<IXdbOperation>) batch;
    }
  }
}
