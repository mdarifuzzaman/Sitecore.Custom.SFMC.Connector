// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.BaseResolveEntityFromXConnectContactStepProcessor`2
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (ResolveObjectSettings), typeof (ResolveIdentifierSettings), typeof (EntityExpressionBuilderSettings), typeof (XConnectContactIdentifierSettings)})]
  public abstract class BaseResolveEntityFromXConnectContactStepProcessor<TEntity, TEntityModel> : 
    BasePipelineStepProcessor,
    ICanResolveObject<string>,
    IPipelineStepProcessor,
    IDataExchangeProcessor<PipelineStep, PipelineContext>
    where TEntity : Entity
    where TEntityModel : EntityModel
  {
    private static ResolveObjectHelper _resolveObjectHelper = new ResolveObjectHelper();

    public BaseResolveEntityFromXConnectContactStepProcessor() => this.ResolveObjectHelper = BaseResolveEntityFromXConnectContactStepProcessor<TEntity, TEntityModel>._resolveObjectHelper;

    protected ResolveObjectHelper ResolveObjectHelper { get; set; }

    public abstract object FindExistingObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    public abstract object CreateNewObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (pipelineStep.GetResolveObjectSettings() == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Pipeline step processing will abort because the pipeline step is missing a plugin.", new string[1]
        {
          "plugin: " + typeof (ResolveObjectSettings).FullName
        });
      }
      else
      {
        string identifierValue = this.ResolveObjectHelper.GetIdentifierValue<string>(new Func<object, PipelineStep, PipelineContext, ILogger, string>(this.ConvertValueToIdentifier), (IPipelineStepProcessor) this, pipelineStep, pipelineContext, logger);
        if (identifierValue == null)
        {
          this.Log(new Action<string>(logger.Error), pipelineContext, "Pipeline step processing will abort because the identifier could not be resolved.", Array.Empty<string>());
        }
        else
        {
          pipelineContext.GetVerificationLogEntry().SourceIdentifier = identifierValue;
          new ResolveObjectOrchestrator<string>().DoResolveObject(identifierValue, (ICanResolveObject<string>) this, pipelineStep, pipelineContext, logger);
        }
      }
    }

    protected abstract TEntity ResolveEntityFromContact(
      Expression<Func<TEntity, bool>> expression,
      Contact contact,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected abstract TEntityModel ConvertEntityToEntityModel(
      TEntity entity,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected abstract TEntityModel CreateEntityModel(
      Contact contact,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    protected virtual ContactModel GetContactModel(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectContactIdentifierSettings plugin = pipelineStep.GetPlugin<XConnectContactIdentifierSettings>();
      return plugin == null ? (ContactModel) null : this.GetObjectFromPipelineContext(plugin.ContactLocation, pipelineContext, logger) as ContactModel;
    }

    protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression(
      string value,
      PipelineStep pipelineStep)
    {
      IEntityExpressionBuilder expressionBuilder = this.GetEntityExpressionBuilder(pipelineStep);
      ExpressionContext expressionContext = new ExpressionContext();
      expressionContext.AddPlugin<EntityFacetPropertySettings>(new EntityFacetPropertySettings()
      {
        PropertyValue = value
      });
      return expressionBuilder.Build<TEntity>(expressionContext);
    }

    protected virtual string ConvertValueToIdentifier(
      object identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return identifierValue?.ToString();
    }

    private IEntityExpressionBuilder GetEntityExpressionBuilder(
      PipelineStep pipelineStep)
    {
      return pipelineStep.GetPlugin<EntityExpressionBuilderSettings>()?.EntityExpressionBuilder;
    }
  }
}
