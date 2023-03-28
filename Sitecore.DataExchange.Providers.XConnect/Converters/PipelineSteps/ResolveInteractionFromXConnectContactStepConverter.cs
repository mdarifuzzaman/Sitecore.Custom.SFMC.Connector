// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.ResolveInteractionFromXConnectContactStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{5CF536B1-D446-4AA7-A2F7-4CF42AC9258D}"})]
  public class ResolveInteractionFromXConnectContactStepConverter : 
    BaseResolveObjectUsingIdentifierFromStepConverter
  {
    public const string FieldNameContactLocation = "ContactLocation";
    public const string FieldNameFacetsToInitialize = "FacetsToInitialize";
    public const string FieldNameFacetName = "FacetName";
    public const string FieldNameFacetType = "FacetType";
    public const string FieldNameEntity = "Entity";
    public const string FieldNameIdentifierValueMatchExpression = "IdentifierValueMatchExpression";

    public ResolveInteractionFromXConnectContactStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      this.AddExpressionSettings(source, pipelineStep);
      this.AddContactIdentifierSettings(source, pipelineStep);
      this.AddEntityFacetSettings(source, pipelineStep);
    }

    protected void AddExpressionSettings(ItemModel source, PipelineStep pipelineStep)
    {
      EntityExpressionBuilderSettings expressionBuilderSettings = new EntityExpressionBuilderSettings()
      {
        EntityExpressionBuilder = this.ConvertReferenceToModel<IEntityExpressionBuilder>(source, "IdentifierValueMatchExpression")
      };
      pipelineStep.AddPlugins((IPlugin) expressionBuilderSettings);
    }

    protected virtual void AddContactIdentifierSettings(ItemModel source, PipelineStep pipelineStep)
    {
      XConnectContactIdentifierSettings identifierSettings = new XConnectContactIdentifierSettings()
      {
        ContactLocation = this.GetGuidValue(source, "ContactLocation")
      };
      pipelineStep.AddPlugins((IPlugin) identifierSettings);
    }

    protected virtual void AddEntityFacetSettings(ItemModel source, PipelineStep pipelineStep)
    {
      List<string> stringList = new List<string>();
      IEnumerable<ItemModel> referencesAsModels = this.GetReferencesAsModels(source, "FacetsToInitialize");
      if (referencesAsModels != null)
      {
        foreach (ItemModel itemModel in referencesAsModels)
        {
          string stringValue = this.GetStringValue(itemModel, "FacetName");
          if (!string.IsNullOrWhiteSpace(stringValue))
            stringList.Add(stringValue);
        }
      }
      EntityFacetSettings entityFacetSettings = new EntityFacetSettings()
      {
        FacetNames = (ICollection<string>) stringList
      };
      pipelineStep.AddPlugins((IPlugin) entityFacetSettings);
    }
  }
}
