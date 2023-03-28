// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.SubmitXConnectBatchConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.Retryers;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{47CB2807-A88C-48C9-9C9E-352E6C630610}", "{445FFC48-ACC4-47E1-ABF2-2111C4149135}"})]
  public class SubmitXConnectBatchConverter : BasePipelineStepConverter
  {
    public const string FieldNameRetryer = "Retryer";
    public const string FieldNameProceedOnException = "ProceedOnException";

    public SubmitXConnectBatchConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      ExceptionHandlingSettings handlingSettings = new ExceptionHandlingSettings()
      {
        ProceedOnException = this.GetBoolValue(source, "ProceedOnException")
      };
      RetrySettings retrySettings = new RetrySettings()
      {
        RetryerOnException = this.ConvertReferenceToModel<IRetryerOnException>(source, "Retryer")
      };
      pipelineStep.AddPlugins((IPlugin) retrySettings, (IPlugin) handlingSettings);
    }
  }
}
