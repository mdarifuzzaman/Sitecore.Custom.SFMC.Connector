// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification.AddXConnectClientToContextVerificationStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification
{
  [RequiredEndpointPlugins(new Type[] {typeof (XConnectClientEndpointSettings)})]
  public class AddXConnectClientToContextVerificationStepProcessor : 
    AddXConnectClientToContextStepProcessor
  {
    protected override XConnectClientPlugin GetXConnectClientPlugin(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectClientPlugin xconnectClientPlugin = base.GetXConnectClientPlugin(pipelineStep, pipelineContext, logger);
      if (xconnectClientPlugin == null)
        return (XConnectClientPlugin) null;
      DefaultXConnectClientHelper helper = new DefaultXConnectClientHelper((IXConnectClientHelperFactory) new DefaultXConnectClientHelperFactory()
      {
        XConnectClientConverter = (IConverter<XConnectClientSettings, IXdbContext>) new DummyXConnectClientConverter()
      });
      return new XConnectClientPlugin(xconnectClientPlugin.ClientSettings, (IXConnectClientHelper) helper);
    }
  }
}
