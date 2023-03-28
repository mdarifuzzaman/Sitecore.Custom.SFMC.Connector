// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.AddXdbReferenceDataClientToContextStepConverter
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

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [SupportedIds(new string[] {"{1AC944B3-994D-48E0-87C3-BAED7F0FE666}"})]
  public class AddXdbReferenceDataClientToContextStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameMinimumBatchSize = "MinimumBatchSize";
    public const string FieldNameEndpoint = "Endpoint";
    public const string FieldNameCommonDataType = "CommonDataType";
    public const string FieldNameCultureDataType = "CultureDataType";
    public const string FieldNameDefinitionDataType = "Type";
    public const string FieldNameDefinitionTypeName = "DefinitionTypeName";
    public const string FieldNameDefinitionTypeVersion = "DefinitionTypeVersion";
    public const string FieldNameBatchEntryStoragePluginLocation = "BatchEntryStoragePluginLocation";

    public AddXdbReferenceDataClientToContextStepConverter(IItemModelRepository repository)
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
      ReferenceDataDefinitionSettings definitionSettings = new ReferenceDataDefinitionSettings()
      {
        DefinitionTypeName = this.GetStringValue(source, "DefinitionTypeName"),
        DefinitionTypeVersion = this.GetShortValue(source, "DefinitionTypeVersion"),
        CommonDataType = this.GetTypeFromReference(source, "CommonDataType", "Type"),
        CultureDataType = this.GetTypeFromReference(source, "CultureDataType", "Type")
      };
      pipelineStep.AddPlugins((IPlugin) endpointSettings, (IPlugin) processingSettings, (IPlugin) entryStorageSettings, (IPlugin) definitionSettings);
    }
  }
}
