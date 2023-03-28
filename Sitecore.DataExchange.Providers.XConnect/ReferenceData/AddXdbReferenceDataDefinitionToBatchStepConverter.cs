// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.AddXdbReferenceDataDefinitionToBatchStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [SupportedIds(new string[] {"{6A5AB14F-700F-43D0-84C6-E23D60689CEA}"})]
  public class AddXdbReferenceDataDefinitionToBatchStepConverter : 
    BaseBatchEntryStorageProcessingStepConverter
  {
    public const string ReferenceDataDefinitionLocationFieldName = "ReferenceDataDefinitionLocation";

    public AddXdbReferenceDataDefinitionToBatchStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      ReferenceDataSettings newPlugin = new ReferenceDataSettings()
      {
        ReferenceDataDefinitionLocation = this.GetGuidValue(source, "ReferenceDataDefinitionLocation")
      };
      pipelineStep.AddPlugin<ReferenceDataSettings>(newPlugin);
    }
  }
}
