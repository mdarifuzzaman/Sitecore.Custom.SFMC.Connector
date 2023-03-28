// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.SubmitSingleXConnectEntityConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{AA2FDA92-D2AB-4E83-A928-0222C2665AAA}"})]
  public class SubmitSingleXConnectEntityConverter : BasePipelineStepConverter
  {
    public const string FieldNameEntityModelLocation = "EntityModelLocation";
    public const string FieldNameEndpoint = "Endpoint";

    public SubmitSingleXConnectEntityConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      this.AddEndpointSettings(source, pipelineStep);
      this.AddResolveObjectSettings(source, pipelineStep);
    }

    protected virtual void AddResolveObjectSettings(ItemModel source, PipelineStep pipelineStep)
    {
      XConnectEntitySettings newPlugin = new XConnectEntitySettings()
      {
        EntityModelLocation = this.GetGuidValue(source, "EntityModelLocation")
      };
      pipelineStep.AddPlugin<XConnectEntitySettings>(newPlugin);
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
