// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.AddEntityToXConnectBatchStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.BatchHandling;
using Sitecore.DataExchange.Hashing;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{E21F2C61-6EA5-41CA-8795-F5167BCCFD46}"})]
  public class AddEntityToXConnectBatchStepConverter : BaseBatchEntryStorageProcessingStepConverter
  {
    public const string FieldNameEntityType = "EntityType";
    public const string FieldNameEntityModelLocation = "EntityModelLocation";
    public const string FieldNameHashGenerator = "HashGenerator";
    public const string FieldNameAddEntityToBatchRules = "AddEntityToBatchRules";
    public const string FieldNameProceedOnException = "ProceedOnException";

    public AddEntityToXConnectBatchStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      ExceptionHandlingSettings newPlugin1 = new ExceptionHandlingSettings()
      {
        ProceedOnException = this.GetBoolValue(source, "ProceedOnException")
      };
      pipelineStep.AddPlugin<ExceptionHandlingSettings>(newPlugin1);
      XConnectEntitySettings newPlugin2 = new XConnectEntitySettings()
      {
        EntityType = this.GetEnumValueFromReference<EntityType>(source, "EntityType", "ItemName"),
        EntityModelLocation = this.GetGuidValue(source, "EntityModelLocation")
      };
      pipelineStep.AddPlugin<XConnectEntitySettings>(newPlugin2);
      HashGeneratorSettings newPlugin3 = new HashGeneratorSettings()
      {
        HashGenerator = this.ConvertReferenceToModel<IHashGenerator>(source, "HashGenerator")
      };
      pipelineStep.AddPlugin<HashGeneratorSettings>(newPlugin3);
      IEnumerable<IAddObjectToBatchRule> models = this.ConvertReferencesToModels<IAddObjectToBatchRule>(source, "AddEntityToBatchRules");
      if (!models.Any<IAddObjectToBatchRule>())
        return;
      AddObjectToBatchRulesPlugin newPlugin4 = new AddObjectToBatchRulesPlugin();
      foreach (IAddObjectToBatchRule objectToBatchRule in models)
        newPlugin4.Rules.Add(objectToBatchRule);
      pipelineStep.AddPlugin<AddObjectToBatchRulesPlugin>(newPlugin4);
    }
  }
}
