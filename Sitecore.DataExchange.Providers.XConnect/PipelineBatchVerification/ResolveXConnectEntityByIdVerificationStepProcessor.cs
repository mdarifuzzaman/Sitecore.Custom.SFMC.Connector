// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification.ResolveXConnectEntityByIdVerificationStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification
{
  public class ResolveXConnectEntityByIdVerificationStepProcessor : 
    ResolveXConnectEntityByIdStepProcessor
  {
    protected override IXConnectClientHelper GetXConnectClientHelper(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (base.GetXConnectClientHelper(pipelineStep, pipelineContext, logger) == null)
        return (IXConnectClientHelper) null;
      return (IXConnectClientHelper) new DefaultXConnectClientHelper((IXConnectClientHelperFactory) new DefaultXConnectClientHelperFactory()
      {
        XConnectClientConverter = (IConverter<XConnectClientSettings, IXdbContext>) new DummyXConnectClientConverter()
      });
    }
  }
}
