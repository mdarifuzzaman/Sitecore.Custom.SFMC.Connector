// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.SubmitSingleXConnectEntityProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class SubmitSingleXConnectEntityProcessor : BasePipelineStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep = null,
      PipelineContext pipelineContext = null,
      ILogger logger = null)
    {
      this.ProcessPipelineStep();
    }

    protected void ProcessPipelineStep() => this.ProcessEntityModel(this.ResolveEntityModel());

    protected virtual EntityModel ResolveEntityModel()
    {
      XConnectEntitySettings plugin = this.PipelineStep.GetPlugin<XConnectEntitySettings>();
      if (plugin != null)
        return this.GetObjectFromPipelineContext(plugin.EntityModelLocation, this.PipelineContext, this.Logger) as EntityModel;
      this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Cannot resolve entity settings from pipeline step.", Array.Empty<string>());
      return (EntityModel) null;
    }

    protected virtual void ProcessEntityModel(EntityModel entityModel)
    {
      if (entityModel == null)
        this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Cannot resolve entity model from location or entity model is null.", Array.Empty<string>());
      else
        this.SubmitEntityToXConnectIfAppropriate(entityModel);
    }

    protected virtual bool ShouldAddEntity(EntityModel entityModel)
    {
      if (this.PipelineContext.GetRepositoryObjectStatusSettings().Status == RepositoryObjectStatus.DoesNotExist)
      {
        this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Entity does not exist in the repository so it should be added.", Array.Empty<string>());
        return true;
      }
      SynchronizationSettings synchronizationSettings = this.PipelineContext.GetSynchronizationSettings();
      if (!synchronizationSettings.IsTargetDirty)
        this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Entity is not dirty so it should not be updated.", new string[1]
        {
          string.Format("entity id: {0}", (object) entityModel.Id)
        });
      return synchronizationSettings.IsTargetDirty;
    }

    protected virtual void OnOperationsAddedToClient(
      IEnumerable<IXdbOperation> operations,
      IXdbContext client,
      int minBatchSize,
      PipelineStep pipelineStep = null,
      PipelineContext pipelineContext = null,
      ILogger logger = null)
    {
    }

    private void SubmitEntityToXConnectIfAppropriate(EntityModel entityModel)
    {
      if (!this.ShouldAddEntity(entityModel))
      {
        this.Log(new Action<string>(this.Logger.Debug), this.PipelineContext, "Entity will not be added or updated ", new string[1]
        {
          string.Format("entity id: {0}", (object) entityModel.Id)
        });
      }
      else
      {
        XConnectClientEndpointSettings plugin = this.PipelineStep.GetEndpointSettings()?.EndpointTo?.GetPlugin<XConnectClientEndpointSettings>();
        if (plugin == null)
          return;
        using (IXdbContext xconnectClient = plugin.ClientHelper.ToXConnectClient(plugin.ClientSettings))
        {
          try
          {
            Entity client = entityModel.AddToClient(xconnectClient, plugin.ClientHelper, new Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger>(this.OnOperationsAddedToClient), 0, this.PipelineStep, this.PipelineContext, this.Logger);
            xconnectClient.Submit();
            if (client == null)
              this.Log(new Action<string>(this.Logger.Error), this.PipelineContext, "No entity was returned when the entity model was added to the xConnect client.", new string[1]
              {
                "entity model internal description: " + entityModel.InternalDescription
              });
            else
              entityModel.Entity = client;
          }
          catch (XdbExecutionException ex)
          {
            this.Log(new Action<string>(this.Logger.Error), this.PipelineContext, "An exception is raised while submitting entity to xConnect.", new string[1]
            {
              "entity model internal description: " + entityModel.InternalDescription
            });
            this.Log(new Action<string>(this.Logger.Error), this.PipelineContext, ex.Message, Array.Empty<string>());
          }
        }
      }
    }
  }
}
