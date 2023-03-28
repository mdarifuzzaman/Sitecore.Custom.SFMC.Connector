// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.SubmitContactToXConnectConverter
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
  [SupportedIds(new string[] {"{F74831DA-7053-4196-A40F-156F0F1A3A64}"})]
  public class SubmitContactToXConnectConverter : BasePipelineStepConverter
  {
    public const string FieldNameContactLocation = "ContactLocation";
    private const string fieldNameEndpoint = "Endpoint";

    public SubmitContactToXConnectConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    public static string FieldNameEndpoint => "Endpoint";

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      this.AddResolveObjectSettings(source, pipelineStep);
      this.AddEndpointSettings(source, pipelineStep);
    }

    protected virtual void AddResolveObjectSettings(ItemModel source, PipelineStep pipelineStep)
    {
      ResolveObjectSettings newPlugin = new ResolveObjectSettings()
      {
        ResolvedObjectLocation = this.GetGuidValue(source, "ContactLocation")
      };
      pipelineStep.AddPlugin<ResolveObjectSettings>(newPlugin);
    }

    protected virtual void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
    {
      EndpointSettings newPlugin = new EndpointSettings();
      Endpoint model = this.ConvertReferenceToModel<Endpoint>(source, SubmitContactToXConnectConverter.FieldNameEndpoint);
      if (model != null)
        newPlugin.EndpointTo = model;
      pipelineStep.AddPlugin<EndpointSettings>(newPlugin);
    }
  }
}
