// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.ResolveXConnectContactByIdentifierStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.Retryers;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{0C843D1D-145B-4F22-8A50-A4BAA1477B05}"})]
  public class ResolveXConnectContactByIdentifierStepConverter : 
    BaseResolveEntityFromXConnectStepConverter
  {
    public const string FieldNameContactIdentifierSourceValueAccessor = "ContactIdentifierSourceValueAccessor";
    public const string FieldNameContactIdentificationLevelObjectLocation = "ContactIdentificationLevelObjectLocation";
    public const string FieldNameContactIdentificationLevelValueAccessor = "ContactIdentificationLevelValueAccessor";
    public const string FieldNameKnownContactValueReaders = "KnownContactValueReaders";
    public const string FieldNameLoadInteractions = "LoadInteractions";
    public const string FieldNameInteractionFacetsToLoad = "InteractionFacetsToLoad";
    public const string FieldNameRetryer = "Retryer";

    public ResolveXConnectContactByIdentifierStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      RetrySettings newPlugin = new RetrySettings()
      {
        RetryerOnException = this.ConvertReferenceToModel<IRetryerOnException>(source, "Retryer")
      };
      pipelineStep.AddPlugin<RetrySettings>(newPlugin);
      this.AddContactIdentifierSettings(source, pipelineStep);
      this.AddLoadInteractionsSettings(source, pipelineStep);
    }

    protected virtual void AddContactIdentifierSettings(ItemModel source, PipelineStep pipelineStep)
    {
      IValueAccessor model = this.ConvertReferenceToModel<IValueAccessor>(source, "ContactIdentifierSourceValueAccessor");
      if (model == null || !(model.ValueReader is ContactIdentifierValueReader valueReader))
        return;
      XConnectContactIdentifierSettings newPlugin = new XConnectContactIdentifierSettings()
      {
        IdentifierSource = valueReader.IdentifierSource,
        ContactIdentificationLevelObjectLocation = this.GetGuidValue(source, "ContactIdentificationLevelObjectLocation"),
        ContactIdentificationLevelValueAccessor = this.ConvertReferenceToModel<IValueAccessor>(source, "ContactIdentificationLevelValueAccessor"),
        KnownContactValueReaders = this.ConvertReferencesToModels<IValueReader>(source, "KnownContactValueReaders")
      };
      pipelineStep.AddPlugin<XConnectContactIdentifierSettings>(newPlugin);
    }

    protected virtual void AddLoadInteractionsSettings(ItemModel source, PipelineStep pipelineStep)
    {
      LoadInteractionsSettings newPlugin = new LoadInteractionsSettings()
      {
        LoadInteractions = this.GetBoolValue(source, "LoadInteractions"),
        FacetNames = (ICollection<string>) new List<string>()
      };
      IEnumerable<ItemModel> referencesAsModels = this.GetReferencesAsModels(source, "InteractionFacetsToLoad");
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

    protected override EntityType GetEntityType(
      ItemModel source,
      PipelineStep pipelineStep)
    {
      return EntityType.Contact;
    }
  }
}
