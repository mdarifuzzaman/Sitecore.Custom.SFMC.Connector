// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.BaseResolveEntityFromXConnectStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (ResolveEntitySettings)})]
  public abstract class BaseResolveEntityFromXConnectStepProcessor : 
    BaseResolveObjectFromRepositoryEndpointStepProcessor<string>
  {
    public override object FindExistingObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      try
      {
        IXdbContext xconnectClient = this.GetXConnectClient(pipelineStep, pipelineContext, logger);
        if (xconnectClient == null)
          return (object) null;
        IXConnectClientHelper xconnectClientHelper = this.GetXConnectClientHelper(pipelineStep, pipelineContext, logger);
        if (xconnectClientHelper == null)
          return (object) null;
        if (!string.IsNullOrWhiteSpace(identifierValue))
          return this.FindExistingObject(xconnectClient, xconnectClientHelper, identifierValue, pipelineStep, pipelineContext, logger);
        this.Log(new Action<string>(logger.Error), pipelineContext, "The identifier value is either null or empty.", Array.Empty<string>());
        return (object) null;
      }
      catch (Exception ex)
      {
        pipelineContext.CriticalError = true;
        throw new XConnectClientException("Exception while reading from xConnect", ex);
      }
    }

    protected virtual ResolveEntitySettings GetResolveEntitySettings(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext)
    {
      return pipelineStep.GetPlugin<ResolveEntitySettings>();
    }

    protected override string ConvertValueToIdentifier(
      object identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return identifierValue.ToString();
    }

    protected virtual IXdbContext GetXConnectClient(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientPlugin xconnectClientPlugin = this.GetXConnectClientPlugin(pipelineStep, pipelineContext, logger);
      if (xconnectClientPlugin == null)
      {
        XConnectClientEndpointSettings plugin = pipelineStep.GetEndpointSettings()?.EndpointFrom?.GetPlugin<XConnectClientEndpointSettings>();
        return plugin?.ClientHelper.ToXConnectClient(plugin.ClientSettings);
      }
      int? threadId = this.GetThreadId(pipelineContext);
      return xconnectClientPlugin.GetClient(!threadId.HasValue ? 0 : threadId.Value);
    }

    protected virtual IXConnectClientHelper GetXConnectClientHelper(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientPlugin xconnectClientPlugin = this.GetXConnectClientPlugin(pipelineStep, pipelineContext, logger);
      if (xconnectClientPlugin != null)
        return xconnectClientPlugin.ClientHelper;
      return pipelineStep.GetEndpointSettings()?.EndpointFrom?.GetPlugin<XConnectClientEndpointSettings>()?.ClientHelper;
    }

    protected abstract object FindExistingObject(
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger);

    private XConnectClientPlugin GetXConnectClientPlugin(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineContext.GetPlugin<XConnectClientPlugin>(true);
    }
  }
}
