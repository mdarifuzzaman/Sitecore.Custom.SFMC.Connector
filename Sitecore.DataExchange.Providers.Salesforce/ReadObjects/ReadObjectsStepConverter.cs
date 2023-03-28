// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ReadObjectsStepConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.Salesforce.Query;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  [SupportedIds(new string[] {"{EB05C247-5209-438B-836D-4B4E2EB9BA37}"})]
  public class ReadObjectsStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameEndpointFrom = "EndpointFrom";
    public const string FieldNameObjectName = "ObjectName";
    public const string FieldNameFieldToRead = "FieldsToRead";
    public const string FieldNameMaxCount = "MaxCount";
    public const string FieldNameFilterExpression = "FilterExpression";
    public const string FieldNameExcludeUseDeltaSettings = "ExcludeUseDeltaSettings";
    public const string FieldNameQueryLocation = "QueryLocation";
    public const string FieldNameContextObjectLocation = "ContextObjectLocation";

    public ReadObjectsStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      IPlugin endpointSettings = (IPlugin) this.GetEndpointSettings(source);
      if (endpointSettings != null)
        pipelineStep.AddPlugin<IPlugin>(endpointSettings);
      IPlugin readObjectsSettings = (IPlugin) this.GetReadObjectsSettings(source);
      if (readObjectsSettings == null)
        return;
      pipelineStep.AddPlugin<IPlugin>(readObjectsSettings);
    }

    protected virtual EndpointSettings GetEndpointSettings(ItemModel source)
    {
      EndpointSettings endpointSettings = new EndpointSettings();
      Endpoint model = this.ConvertReferenceToModel<Endpoint>(source, "EndpointFrom");
      if (model != null)
        endpointSettings.EndpointFrom = model;
      return endpointSettings;
    }

    protected virtual ReadObjectsSettings GetReadObjectsSettings(ItemModel source)
    {
      ReadObjectsSettings readObjectsSettings = new ReadObjectsSettings()
      {
        ObjectName = this.GetStringValue(source, "ObjectName"),
        MaxCount = this.GetIntValue(source, "MaxCount"),
        FilterExpression = this.ConvertReferenceToModel<FilterExpressionDescriptor>(source, "FilterExpression"),
        ExcludeUseDeltaSettings = this.GetBoolValue(source, "ExcludeUseDeltaSettings"),
        QueryLocation = this.GetGuidValue(source, "QueryLocation"),
        ContextObjectLocation = this.GetGuidValue(source, "ContextObjectLocation")
      };
      IEnumerable<ICollection<string>> models = this.ConvertReferencesToModels<ICollection<string>>(source, "FieldsToRead");
      if (models != null)
      {
        foreach (IEnumerable<string> strings in models)
        {
          foreach (string str in strings)
            readObjectsSettings.FieldsToRead.Add(str);
        }
      }
      return readObjectsSettings;
    }
  }
}
