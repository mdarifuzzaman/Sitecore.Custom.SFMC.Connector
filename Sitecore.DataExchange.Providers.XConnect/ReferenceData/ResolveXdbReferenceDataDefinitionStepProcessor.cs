// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ResolveXdbReferenceDataDefinitionStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Xdb.ReferenceData.Core;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (ReferenceDataDefinitionSettings)})]
  public class ResolveXdbReferenceDataDefinitionStepProcessor : 
    BasePipelineStepProcessor,
    ICanResolveObject<string>,
    IPipelineStepProcessor,
    IDataExchangeProcessor<PipelineStep, PipelineContext>
  {
    private static ResolveObjectHelper _resolveObjectHelper = new ResolveObjectHelper();

    public ResolveXdbReferenceDataDefinitionStepProcessor()
    {
      this.ReflectionUtil = (IReflectionUtil) Sitecore.DataExchange.DataAccess.Reflection.ReflectionUtil.Instance;
      this.ResolveObjectHelper = ResolveXdbReferenceDataDefinitionStepProcessor._resolveObjectHelper;
    }

    public IReflectionUtil ReflectionUtil { get; set; }

    protected ResolveObjectHelper ResolveObjectHelper { get; set; }

    public virtual object FindExistingObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ReferenceDataClientPlugin plugin = pipelineContext.GetPlugin<ReferenceDataClientPlugin>(true);
      ReferenceDataHelperContext dataHelperContext = this.GetReferenceDataHelperContext(identifierValue, plugin, pipelineStep, pipelineContext, logger);
      if (dataHelperContext == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the reference data helper context.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      BaseDefinition definition = plugin.ReferenceDataHelper.GetDefinition(dataHelperContext, logger);
      if (definition == null || plugin.ReferenceDataHelper.InitializePropertiesIfNeeded(definition, dataHelperContext, logger))
        return (object) definition;
      this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to initialize the reference data definition.", Array.Empty<string>());
      pipelineContext.CriticalError = true;
      return (object) null;
    }

    public virtual object CreateNewObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ReferenceDataClientPlugin plugin = pipelineContext.GetPlugin<ReferenceDataClientPlugin>(true);
      if (plugin.GetClient(0) == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The client was not resolved from the plugin.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      DefinitionKey definitionKey = this.GetDefinitionKey(identifierValue, plugin, pipelineStep, pipelineContext, logger);
      if (definitionKey == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The definition key object was not created.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      if (!(this.ReflectionUtil.CreateObjectFromGenericTypeDefinition(typeof (Definition<,>), new Type[2]
      {
        plugin.CommonDataType,
        plugin.CultureDataType
      }, (object) definitionKey) is BaseDefinition genericTypeDefinition))
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "A new definition item could not be created.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      ReferenceDataHelperContext dataHelperContext = this.GetReferenceDataHelperContext(identifierValue, plugin, pipelineStep, pipelineContext, logger);
      if (dataHelperContext == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to get the reference data helper context.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (object) null;
      }
      if (plugin.ReferenceDataHelper.InitializePropertiesIfNeeded(genericTypeDefinition, dataHelperContext, logger))
        return (object) genericTypeDefinition;
      this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to initialize the reference data definition.", Array.Empty<string>());
      pipelineContext.CriticalError = true;
      return (object) null;
    }

    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      string identifierValue = this.ResolveObjectHelper.GetIdentifierValue<string>(new Func<object, PipelineStep, PipelineContext, ILogger, string>(this.ConvertValueToIdentifier), (IPipelineStepProcessor) this, pipelineStep, pipelineContext, logger);
      if (string.IsNullOrWhiteSpace(identifierValue))
        this.Log(new Action<string>(logger.Error), pipelineContext, "Pipeline step processing will abort because the identifier could not be resolved.", Array.Empty<string>());
      else
        new ResolveObjectOrchestrator<string>().DoResolveObject(identifierValue, (ICanResolveObject<string>) this, pipelineStep, pipelineContext, logger);
    }

    protected virtual ReferenceDataHelperContext GetReferenceDataHelperContext(
      string identifierValue,
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IReferenceDataClient client = plugin.GetClient(0);
      if (client == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The client was not resolved from the plugin.", Array.Empty<string>());
        pipelineContext.CriticalError = true;
        return (ReferenceDataHelperContext) null;
      }
      ReferenceDataDefinitionSettings plugin1 = pipelineStep.GetPlugin<ReferenceDataDefinitionSettings>();
      return new ReferenceDataHelperContext()
      {
        Client = client,
        CommonDataType = plugin.CommonDataType,
        CultureDataType = plugin.CultureDataType,
        DefinitionTypeName = plugin1.DefinitionTypeName,
        InitializeCommonData = plugin1.InitializeCommonData,
        InitializeCultureData = plugin1.InitializeCultureData,
        Moniker = identifierValue,
        ActivateDefinition = plugin1.ActivateDefinition
      };
    }

    protected virtual DefinitionKey GetDefinitionKey(
      string moniker,
      ReferenceDataClientPlugin plugin,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return new DefinitionKey(moniker, plugin.DefinitionTypeKey, plugin.DefinitionTypeVersion);
    }

    protected virtual string ConvertValueToIdentifier(
      object identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return identifierValue?.ToString();
    }
  }
}
