// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.BaseSubmitEntityToXConnectStepProcessor`2
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredEndpointPlugins(new Type[] {typeof (XConnectClientEndpointSettings)})]
  [RequiredPipelineStepPlugins(new Type[] {typeof (ResolveObjectSettings)})]
  [RequiredPipelineContextPlugins(new Type[] {typeof (SynchronizationSettings)})]
  public abstract class BaseSubmitEntityToXConnectStepProcessor<TEntityModel, TEntity> : 
    BasePipelineStepProcessor
    where TEntityModel : EntityModel
    where TEntity : Entity
  {
    protected abstract TEntity ConvertEntityModelToEntity(
      TEntityModel entityModel,
      IXConnectClientHelper clientHelper,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected abstract IEnumerable<IXdbOperation> AddEntityToBatch(
      TEntityModel entityModel,
      TEntity entity,
      IXdbContext client,
      IXConnectClientHelper helper,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (this.GetBatchEntryStorageLocation(pipelineStep, pipelineContext) == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the location of the xConnect client from the pipeline step.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
      }
      else
      {
        IXdbContext xconnectClient = this.GetXConnectClient(pipelineStep, pipelineContext, logger);
        if (xconnectClient == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get xConnect client.", Array.Empty<string>());
        }
        else
        {
          IXConnectClientHelper xconnectClientHelper = this.GetXConnectClientHelper(pipelineStep, pipelineContext, logger);
          if (xconnectClientHelper == null)
          {
            this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get xConnect client helper.", Array.Empty<string>());
          }
          else
          {
            TEntityModel entityModel = this.ResolveEntityModel(pipelineStep, pipelineContext, logger);
            if ((object) entityModel == null)
            {
              this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot resolve entity model from location.", Array.Empty<string>());
            }
            else
            {
              XdbOperationResultSettings operationResultSettings = new XdbOperationResultSettings();
              TEntity entity = this.ConvertEntityModelToEntity(entityModel, xconnectClientHelper, pipelineStep, pipelineContext, logger);
              if ((object) entity == null)
              {
                this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot resolve entity from entity model.", Array.Empty<string>());
              }
              else
              {
                try
                {
                  IEnumerable<IXdbOperation> batch = this.AddEntityToBatch(entityModel, entity, xconnectClient, xconnectClientHelper, pipelineStep, pipelineContext, logger);
                  if (batch == null || !batch.Any<IXdbOperation>())
                  {
                    operationResultSettings.Success = false;
                  }
                  else
                  {
                    xconnectClient.SubmitAsync().Wait();
                    operationResultSettings.Success = true;
                  }
                }
                catch (Exception ex)
                {
                  this.LogException(ex, new Action<string>(logger.Error), pipelineContext, "Exception while submitting contact to xConnect.", new string[1]
                  {
                    string.Format("contact id: {0}", (object) entity.Id)
                  });
                  operationResultSettings.Exception = ex;
                  operationResultSettings.Success = false;
                }
                pipelineContext.AddPlugins((IPlugin) operationResultSettings);
              }
            }
          }
        }
      }
    }

    protected virtual TEntityModel ResolveEntityModel(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return this.GetObjectFromPipelineContext(pipelineStep.GetPlugin<ResolveObjectSettings>().ResolvedObjectLocation, pipelineContext, logger) as TEntityModel;
    }

    protected virtual IXConnectClientHelper GetXConnectClientHelper(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return this.GetXConnectClientEndpointSettings(pipelineStep, pipelineContext, logger)?.ClientHelper;
    }

    protected virtual IXdbContext GetXConnectClient(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientEndpointSettings endpointSettings = this.GetXConnectClientEndpointSettings(pipelineStep, pipelineContext, logger);
      if (endpointSettings == null)
        return (IXdbContext) null;
      XConnectClientSettings clientSettings = endpointSettings.ClientSettings;
      if (clientSettings == null)
        return (IXdbContext) null;
      return endpointSettings.ClientHelper?.ToXConnectClient(clientSettings);
    }

    private XConnectClientEndpointSettings GetXConnectClientEndpointSettings(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetEndpointSettings().EndpointTo.GetPlugin<XConnectClientEndpointSettings>();
    }
  }
}
