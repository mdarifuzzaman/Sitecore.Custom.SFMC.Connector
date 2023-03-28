// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Queues.ObjectQueueStepConvertor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.Retryers;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.Queues
{
  [SupportedIds(new string[] {"{C7A6F264-B9F7-4A60-962B-11004FAFB9B8}"})]
  public class ObjectQueueStepConvertor : BaseBatchEntryStorageProcessingStepConverter
  {
    public const string FieldNameObjectName = "ObjectName";
    public const string FieldNameSitecoreIdFieldName = "SitecoreIdFieldName";

    public ObjectQueueStepConvertor(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      RetrySettings retrySettings = new RetrySettings()
      {
        RetryerOnException = this.ConvertReferenceToModel<IRetryerOnException>(source, "Retryer")
      };
      QueueOperationSettings operationSettings = new QueueOperationSettings()
      {
        ObjectName = this.GetStringValue(source, "ObjectName"),
        SitecoreIdFieldName = this.GetStringValue(source, "SitecoreIdFieldName")
      };
      pipelineStep.AddPlugins((IPlugin) operationSettings, (IPlugin) retrySettings);
    }
  }
}
