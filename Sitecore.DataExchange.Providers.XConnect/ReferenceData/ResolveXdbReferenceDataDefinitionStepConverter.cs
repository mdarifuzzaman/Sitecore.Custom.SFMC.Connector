// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ResolveXdbReferenceDataDefinitionStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [SupportedIds(new string[] {"{11662489-5669-41B4-8BBE-7C74FDF32806}"})]
  public class ResolveXdbReferenceDataDefinitionStepConverter : 
    BaseResolveObjectUsingIdentifierFromStepConverter
  {
    public const string DefinitionTypeNameFieldName = "DefinitionTypeName";
    public const string InitializeCommonDataFieldName = "InitializeCommonData";
    public const string InitializeCultureDataFieldName = "InitializeCultureData";
    public const string ActivateDefinitionFieldName = "ActivateDefinition";

    public ResolveXdbReferenceDataDefinitionStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      this.AddReferenceDataDefinitionSettings(source, pipelineStep);
      this.AddResolveObjectSettings(source, pipelineStep);
    }

    private void AddReferenceDataDefinitionSettings(ItemModel source, PipelineStep pipelineStep)
    {
      ReferenceDataDefinitionSettings newPlugin = new ReferenceDataDefinitionSettings()
      {
        DefinitionTypeName = this.GetStringValue(source, "DefinitionTypeName"),
        InitializeCommonData = this.GetBoolValue(source, "InitializeCommonData"),
        InitializeCultureData = this.GetBoolValue(source, "InitializeCultureData"),
        ActivateDefinition = this.GetBoolValue(source, "ActivateDefinition")
      };
      pipelineStep.AddPlugin<ReferenceDataDefinitionSettings>(newPlugin);
    }

    private void AddResolveObjectSettings(ItemModel source, PipelineStep pipelineStep)
    {
      IConverter<ItemModel, ResolveObjectSettings> settingsConverter = this.GetResolveObjectSettingsConverter(source, pipelineStep);
      if (settingsConverter == null)
        return;
      ConvertResult<ResolveObjectSettings> convertResult = settingsConverter.Convert(source);
      if (!convertResult.WasConverted || convertResult.ConvertedValue == null)
        return;
      pipelineStep.AddPlugin<ResolveObjectSettings>(convertResult.ConvertedValue);
    }
  }
}
