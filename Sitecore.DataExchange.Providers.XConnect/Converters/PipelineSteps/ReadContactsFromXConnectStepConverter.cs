// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.ReadContactsFromXConnectStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{A0FA5B87-6FF3-4AE7-9313-6C1F4A53984B}"})]
  public class ReadContactsFromXConnectStepConverter : BaseResolveObjectFromEndpointStepConverter
  {
    public const string FieldNameContactFacetsToRead = "ContactFacetsToRead";
    public const string FieldNameInteractionFacetsToRead = "InteractionFacetsToRead";
    public const string FieldNameLoadInteractions = "LoadInteractions";
    public const string FieldNameContactBatchSize = "ContactBatchSize";
    public const string FieldNameContactMaxCount = "ContactMaxCount";
    public const string FieldNameInteractionMaxCount = "InteractionMaxCount";
    public const string FieldNameContactFilter = "ContactFilter";

    public ReadContactsFromXConnectStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      this.AddLoadInteractionsSettings(source, pipelineStep);
      this.AddReadEntitySettings(source, pipelineStep);
    }

    protected virtual void AddReadEntitySettings(ItemModel source, PipelineStep pipelineStep)
    {
      ReadEntitySettings newPlugin = new ReadEntitySettings()
      {
        BatchSize = this.GetIntValue(source, "ContactBatchSize"),
        MaxCount = this.GetIntValue(source, "ContactMaxCount"),
        FacetNames = new List<string>(0),
        EntityExpressionBuilder = this.ConvertReferenceToModel<IEntityExpressionBuilder>(source, "ContactFilter")
      };
      IEnumerable<ItemModel> referencesAsModels = this.GetReferencesAsModels(source, "ContactFacetsToRead");
      if (referencesAsModels != null)
      {
        foreach (ItemModel itemModel in referencesAsModels)
        {
          string stringValue = this.GetStringValue(itemModel, "FacetName");
          newPlugin.FacetNames.Add(stringValue);
        }
      }
      pipelineStep.AddPlugin<ReadEntitySettings>(newPlugin);
    }

    protected virtual void AddLoadInteractionsSettings(ItemModel source, PipelineStep pipelineStep)
    {
      LoadInteractionsSettings newPlugin = new LoadInteractionsSettings()
      {
        MaxCount = this.GetIntValue(source, "InteractionMaxCount"),
        LoadInteractions = this.GetBoolValue(source, "LoadInteractions"),
        FacetNames = (ICollection<string>) new List<string>()
      };
      IEnumerable<ItemModel> referencesAsModels = this.GetReferencesAsModels(source, "InteractionFacetsToRead");
      if (referencesAsModels != null)
      {
        foreach (ItemModel itemModel in referencesAsModels)
        {
          string stringValue = this.GetStringValue(itemModel, "FacetName");
          newPlugin.FacetNames.Add(stringValue);
        }
      }
      pipelineStep.AddPlugin<LoadInteractionsSettings>(newPlugin);
    }
  }
}
