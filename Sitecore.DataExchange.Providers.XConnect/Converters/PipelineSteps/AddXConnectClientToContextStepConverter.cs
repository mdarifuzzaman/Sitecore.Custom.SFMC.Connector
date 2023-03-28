// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.AddXConnectClientToContextStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{7F28D8E6-0FF0-463D-978F-8F3E13EE1B44}"})]
  public class AddXConnectClientToContextStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameBatchEntryStoragePluginLocation = "BatchEntryStoragePluginLocation";
    public const string FieldNameEndpoint = "Endpoint";
    public const string FieldNameMinimumBatchSize = "MinimumBatchSize";

    public AddXConnectClientToContextStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      EndpointSettings endpointSettings = new EndpointSettings()
      {
        EndpointTo = this.ConvertReferenceToModel<Endpoint>(source, "Endpoint")
      };
      BatchEntryStorageSettings entryStorageSettings = new BatchEntryStorageSettings()
      {
        PluginLocation = this.GetGuidValue(source, "BatchEntryStoragePluginLocation")
      };
      BatchProcessingSettings processingSettings = new BatchProcessingSettings()
      {
        MinimumBatchSize = this.GetIntValue(source, "MinimumBatchSize")
      };
      pipelineStep.AddPlugins((IPlugin) endpointSettings, (IPlugin) entryStorageSettings, (IPlugin) processingSettings);
    }
  }
}
