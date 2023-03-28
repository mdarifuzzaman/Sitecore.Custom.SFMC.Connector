// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.BaseResolveEntityFromXConnectStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  public abstract class BaseResolveEntityFromXConnectStepConverter : 
    BaseResolveObjectFromEndpointStepConverter
  {
    public const string FieldNameFacetName = "FacetName";
    public const string FieldNameFacetType = "FacetType";
    public const string FieldNameEntity = "Entity";
    public const string FieldNameUseConnectionSettingsFromContext = "UseConnectionSettingsFromContext";
    public const string FieldNameFacetsToRead = "FacetsToRead";
    public const string FieldNameDoNotConvertToEntityModel = "DoNotConvertToEntityModel";

    protected BaseResolveEntityFromXConnectStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      this.AddResolveEntitySettings(source, pipelineStep);
    }

    protected virtual void AddResolveEntitySettings(ItemModel source, PipelineStep pipelineStep)
    {
      EntityType entityType = this.GetEntityType(source, pipelineStep);
      ResolveEntitySettings resolveEntitySettings = new ResolveEntitySettings()
      {
        EntityType = entityType,
        FacetNames = new List<string>(),
        DoNotConvertToEntityModel = this.GetBoolValue(source, "DoNotConvertToEntityModel"),
        UseConnectionSettingsFromContext = this.GetBoolValue(source, "UseConnectionSettingsFromContext")
      };
      IEnumerable<ItemModel> referencesAsModels = this.GetReferencesAsModels(source, "FacetsToRead");
      if (referencesAsModels != null)
      {
        foreach (ItemModel itemModel in referencesAsModels)
        {
          string stringValue = this.GetStringValue(itemModel, "FacetName");
          if (!string.IsNullOrWhiteSpace(stringValue))
            resolveEntitySettings.FacetNames.Add(stringValue);
        }
      }
      pipelineStep.AddPlugins((IPlugin) resolveEntitySettings);
    }

    protected abstract EntityType GetEntityType(
      ItemModel source,
      PipelineStep pipelineStep);
  }
}
