// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.AddEntityToXConnectBatchStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Hashing;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.VerificationLog;
using Sitecore.Framework.Conditions;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (BatchEntryStorageSettings)})]
  [RequiredPipelineContextPlugins(new Type[] {typeof (RepositoryObjectStatusSettings)})]
  public class AddEntityToXConnectBatchStepProcessor : BaseSubmitXConnectBatchStepProcessor
  {
    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      this.ProcessEntityModel(this.ResolveEntityModel(pipelineStep, pipelineContext, logger), pipelineStep, pipelineContext, logger);
    }

    protected virtual EntityModel ResolveEntityModel(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectEntitySettings plugin = pipelineStep.GetPlugin<XConnectEntitySettings>();
      return plugin == null ? (EntityModel) null : this.GetObjectFromPipelineContext(plugin.EntityModelLocation, pipelineContext, logger) as EntityModel;
    }

    protected virtual bool ShouldGenerateAndRecordHash(
      EntityModel entityModel,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      VerificationLogSettings plugin = pipelineContext?.PipelineBatchContext?.CurrentPipelineBatch?.GetPlugin<VerificationLogSettings>();
      return plugin != null && plugin.VerificationEnabled;
    }

    protected virtual void RecordHash(
      EntityModel entityModel,
      EntityType entityType,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IHashGenerator hashGenerator = this.GetHashGenerator(pipelineStep, pipelineContext, logger);
      Condition.Ensures<IHashGenerator>(hashGenerator).IsNotNull<IHashGenerator>();
      VerificationLogSettings verificationLogSettings = this.GetVerificationLogSettings(pipelineContext.GetCurrentPipelineBatch());
      string json;
      string etag = hashGenerator.ComputeETag((object) entityModel, out json);
      VerificationLogEntry verificationLogEntry1 = pipelineContext.GetVerificationLogEntry();
      VerificationLogEntry verificationLogEntry2 = verificationLogEntry1;
      string str;
      if (entityModel == null)
      {
        str = (string) null;
      }
      else
      {
        Entity entity = entityModel.Entity;
        if (entity == null)
        {
          str = (string) null;
        }
        else
        {
          // ISSUE: explicit non-virtual call
          Guid? id = entity.Id;
          ref Guid? local = ref id;
          str = local.HasValue ? local.GetValueOrDefault().ToString() : (string) null;
        }
      }
      verificationLogEntry2.TargetIdentifier = str;
      verificationLogEntry1.Hash = etag;
      verificationLogEntry1.TargetType = entityType.ToString("G");
      verificationLogEntry1.TargetObject = (object) entityModel?.Entity;
      verificationLogEntry1.ThreadId = pipelineContext.GetCurrentThreadId();
      verificationLogEntry1.Json = verificationLogSettings == null || !verificationLogSettings.SaveJson ? string.Empty : json;
      this.GetVerificationLogCollection(pipelineContext).Add((IVerificationLogEntry) verificationLogEntry1);
    }

    protected virtual IHashGenerator GetHashGenerator(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<HashGeneratorSettings>()?.HashGenerator;
    }

    protected virtual void ProcessEntityModel(
      EntityModel entityModel,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (entityModel == null)
      {
        this.Log(new Action<string>(logger.Debug), pipelineContext, "Cannot resolve entity model from location or entity model is null.", Array.Empty<string>());
      }
      else
      {
        XConnectEntitySettings plugin = pipelineStep.GetPlugin<XConnectEntitySettings>();
        if (plugin == null)
          this.Log(new Action<string>(logger.Debug), pipelineContext, "Cannot resolve entity settings from pipeline step.", Array.Empty<string>());
        else
          this.AddEntityToBatchIfAppropriate(plugin.EntityType, entityModel, pipelineStep, pipelineContext, logger);
      }
    }

    protected virtual void AddEntityToBatchIfAppropriate(
      EntityType entityType,
      EntityModel entityModel,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (!this.ShouldAddEntityToBatch(entityModel, pipelineStep, pipelineContext, logger))
      {
        this.Log(new Action<string>(logger.Debug), pipelineContext, "Entity will not be added to the batch.", new string[1]
        {
          string.Format("entity id: {0}", (object) entityModel.Id)
        });
      }
      else
      {
        IHasPlugins entryStorageLocation = this.GetBatchEntryStorageLocation(pipelineStep, pipelineContext);
        if (entryStorageLocation == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the location of the xConnect client from the pipeline step.", Array.Empty<string>());
          pipelineContext.CriticalError = true;
        }
        else
        {
          XConnectClientPlugin plugin1 = entryStorageLocation.GetPlugin<XConnectClientPlugin>(true);
          if (plugin1 == null)
          {
            this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get xConnect client plugin from the location object or its parents.", Array.Empty<string>());
            pipelineContext.CriticalError = true;
          }
          else
          {
            AddObjectToBatchRulesPlugin plugin2 = pipelineStep.GetPlugin<AddObjectToBatchRulesPlugin>();
            if (plugin2 != null && plugin2.Rules.Count > 0 && !this.DoAddObjectToBatchRulesPass(entityType, entityModel, plugin2, pipelineContext, logger))
              return;
            this.AddEntityToBatch(plugin1, entityType, entityModel, pipelineStep, pipelineContext, logger);
          }
        }
      }
    }

    protected virtual bool DoAddObjectToBatchRulesPass(
      EntityType entityType,
      EntityModel entityModel,
      AddObjectToBatchRulesPlugin rulesPlugin,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      foreach (IAddObjectToBatchRule rule in (IEnumerable<IAddObjectToBatchRule>) rulesPlugin.Rules)
      {
        if (!rule.ShouldAddObjectToBatch((object) entityModel))
        {
          List<string> stringList = new List<string>();
          stringList.Add(string.Format("entity type: {0}", (object) entityType));
          if (entityModel.Id != Guid.Empty)
            stringList.Add(string.Format("entity id: {0}", (object) entityModel.Id));
          stringList.Add("internal description: " + entityModel.InternalDescription);
          stringList.Add("rule identifier: " + rule.Identifier);
          this.Log(new Action<string>(logger.Error), pipelineContext, "The entity model will not be added to the batch because one of the rules did not pass.", stringList.ToArray());
          return false;
        }
      }
      return true;
    }

    protected virtual IXdbContext GetXConnectClient(
      XConnectClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      int? threadId = this.GetThreadId(pipelineContext);
      return plugin.GetClient(!threadId.HasValue ? 0 : threadId.Value);
    }

    protected virtual void AddEntityToBatch(
      XConnectClientPlugin plugin,
      EntityType entityType,
      EntityModel entityModel,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IXdbContext xconnectClient = this.GetXConnectClient(plugin, pipelineStep, pipelineContext, logger);
      if (xconnectClient == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot get xConnect client from the plugin.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
      }
      else
      {
        ExceptionHandlingSettings plugin1 = pipelineStep.GetPlugin<ExceptionHandlingSettings>();
        try
        {
          Entity client = entityModel.AddToClient(xconnectClient, plugin.ClientHelper, new Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger>(this.OnOperationsAddedToClient), plugin.MinimumBatchSize, pipelineStep, pipelineContext, logger);
          if (client == null)
          {
            this.Log(new Action<string>(logger.Error), pipelineContext, "No entity was returned when the entity model was added to the xConnect client.", new string[1]
            {
              "entity model internal description: " + entityModel.InternalDescription
            });
          }
          else
          {
            entityModel.Entity = client;
            if (!this.ShouldGenerateAndRecordHash(entityModel, pipelineStep, pipelineContext, logger))
              return;
            this.RecordHash(entityModel, entityType, pipelineStep, pipelineContext, logger);
          }
        }
        catch (Exception ex)
        {
          this.LogException(ex, new Action<string>(logger.Error), pipelineContext, "An error occurred when trying to add the entity to the xConnect client.", new string[1]
          {
            "entity model internal description: " + entityModel.InternalDescription
          });
          if (plugin1 != null && plugin1.ProceedOnException)
            return;
          pipelineContext.CriticalError = true;
        }
      }
    }

    protected virtual bool ShouldAddEntityToBatch(
      EntityModel entityModel,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (pipelineContext.GetRepositoryObjectStatusSettings().Status == RepositoryObjectStatus.DoesNotExist)
      {
        this.Log(new Action<string>(logger.Debug), pipelineContext, "Entity does not exist in the repository so it should be added to the batch.", Array.Empty<string>());
        return true;
      }
      SynchronizationSettings synchronizationSettings = pipelineContext.GetSynchronizationSettings();
      if (!synchronizationSettings.IsTargetDirty)
        this.Log(new Action<string>(logger.Debug), pipelineContext, "Entity is not dirty so it should not be added to the batch.", new string[1]
        {
          string.Format("entity id: {0}", (object) entityModel.Id)
        });
      return synchronizationSettings.IsTargetDirty;
    }

    protected virtual void OnOperationsAddedToClient(
      IEnumerable<IXdbOperation> operations,
      IXdbContext client,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (client == null || minBatchSize == 0 || client.DirectOperations.Count < minBatchSize)
        return;
      this.SubmitBatch(client, pipelineStep, pipelineContext, logger);
    }
  }
}
