// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Models.ReadOnlyEntityModel`1
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Models
{
  public abstract class ReadOnlyEntityModel<TEntity> : EntityModel where TEntity : Entity
  {
    public override Entity AddToClient(
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationsAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      TEntity entity = this.GetEntity(helper);
      IEnumerable<IXdbOperation> operationsForEntity = this.GetOperationsForEntity(entity, client, logger);
      if (operationsForEntity == null || !operationsForEntity.Any<IXdbOperation>())
      {
        logger.Error("No operations were created for the entity, meaning the entity was not added to the xConnect client. (entity type: " + entity.GetType().Name + ", entity model internal description: " + this.InternalDescription + ")");
        return (Entity) null;
      }
      onOperationsAddedToClient(operationsForEntity, client, minBatchSize, pipelineStep, pipelineContext, logger);
      this.AddFacets(this.GetEntityReference(entity), client, helper, onOperationsAddedToClient, minBatchSize, pipelineStep, pipelineContext, logger);
      return (Entity) entity;
    }

    protected abstract TEntity GetEntity(IXConnectClientHelper helper);

    protected abstract IEntityReference GetEntityReference(TEntity entity);

    protected abstract IEnumerable<IXdbOperation> GetOperationsForEntity(
      TEntity entity,
      IXdbContext client,
      ILogger logger);

    protected override void AddFacets(
      IEntityReference entity,
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
    }
  }
}
