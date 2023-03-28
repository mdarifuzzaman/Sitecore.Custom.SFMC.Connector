// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.SubmitInteractionToXConnectConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{A4BCBE33-68B8-418F-9655-53F8DB1F0E24}"})]
  public class SubmitInteractionToXConnectConverter : BasePipelineStepConverter
  {
    public const string FieldNameInteractionLocation = "InteractionLocation";
    public const string FieldNameEndpoint = "Endpoint";

    public SubmitInteractionToXConnectConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      this.AddResolveObjectSettings(source, pipelineStep);
      this.AddEndpointSettings(source, pipelineStep);
    }

    protected virtual void AddResolveObjectSettings(ItemModel source, PipelineStep pipelineStep)
    {
      ResolveObjectSettings newPlugin = new ResolveObjectSettings()
      {
        ResolvedObjectLocation = this.GetGuidValue(source, "InteractionLocation")
      };
      pipelineStep.AddPlugin<ResolveObjectSettings>(newPlugin);
    }

    protected virtual void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
    {
      EndpointSettings newPlugin = new EndpointSettings();
      Endpoint model = this.ConvertReferenceToModel<Endpoint>(source, "Endpoint");
      if (model != null)
        newPlugin.EndpointTo = model;
      pipelineStep.AddPlugin<EndpointSettings>(newPlugin);
    }
  }
}
