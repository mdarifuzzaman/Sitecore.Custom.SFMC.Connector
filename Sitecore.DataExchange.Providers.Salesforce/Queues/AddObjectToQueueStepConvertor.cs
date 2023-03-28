// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.AddObjectToQueueStepConvertor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  [SupportedIds(new string[] {"{6BB19C5B-8E29-4E21-8C61-AE0AEA5DF2A2}"})]
  public class AddObjectToQueueStepConvertor : ObjectQueueStepConvertor
  {
    public const string FieldNameObjectLocation = "ObjectLocation";

    public AddObjectToQueueStepConvertor(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      BatchEntryStorageSettings entryStorageSettings = new BatchEntryStorageSettings()
      {
        PluginLocation = this.GetGuidValue(source, "BatchEntryStoragePluginLocation"),
        LocationForNewEntry = this.GetGuidValue(source, "ObjectLocation")
      };
      pipelineStep.AddPlugins((IPlugin) entryStorageSettings);
    }
  }
}
