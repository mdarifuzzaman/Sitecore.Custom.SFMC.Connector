// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.CreateObjectQueueStepConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  [SupportedIds(new string[] {"{AFC44E1B-A346-4E41-9197-54680CA9F572}"})]
  public class CreateObjectQueueStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameBatchEntryStoragePluginLocation = "BatchEntryStoragePluginLocation";
    public const string FieldNameMinimumBatchSize = "MinimumBatchSize";
    public const string FieldNameEndpointTo = "EndpointTo";

    public CreateObjectQueueStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      BatchEntryStorageSettings entryStorageSettings = this.GetBatchEntryStorageSettings(source);
      if (entryStorageSettings != null)
        pipelineStep.AddPlugin<BatchEntryStorageSettings>(entryStorageSettings);
      EndpointSettings endpointSettings = this.GetEndpointSettings(source);
      if (endpointSettings == null)
        return;
      pipelineStep.AddPlugin<EndpointSettings>(endpointSettings);
    }

    protected virtual BatchEntryStorageSettings GetBatchEntryStorageSettings(
      ItemModel source)
    {
      return new BatchEntryStorageSettings()
      {
        PluginLocation = this.GetGuidValue(source, "BatchEntryStoragePluginLocation"),
        DefaultMinimumBatchSize = this.GetIntValue(source, "MinimumBatchSize")
      };
    }

    protected virtual EndpointSettings GetEndpointSettings(ItemModel source) => new EndpointSettings()
    {
      EndpointTo = this.ConvertReferenceToModel<Endpoint>(source, "EndpointTo")
    };
  }
}
