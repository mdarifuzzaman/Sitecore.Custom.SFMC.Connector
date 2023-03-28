// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ResolveObjectStepConvertor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  [SupportedIds(new string[] {"{70413326-C6C1-4BEB-9369-E0E51A8E243A}"})]
  public class ResolveObjectStepConvertor : BaseResolveObjectUsingIdentifierFromStepConverter
  {
    public const string FieldNameObjectName = "ObjectName";
    public const string FieldNameFieldToRead = "FieldsToRead";
    public const string FieldNameEndpointFrom = "EndpointFrom";
    public const string FieldNameIdentifierFieldName = "IdFieldName";

    public ResolveObjectStepConvertor(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      pipelineStep.AddPlugins((IPlugin) this.GetEndpointSettings(source));
      pipelineStep.AddPlugins((IPlugin) this.GetReadObjectsSettings(source));
    }

    protected ReadObjectsSettings GetReadObjectsSettings(ItemModel source)
    {
      ReadObjectsSettings readObjectsSettings = new ReadObjectsSettings()
      {
        ObjectName = this.GetStringValue(source, "ObjectName"),
        IdentifierFieldName = this.GetStringValue(source, "IdFieldName")
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

    protected virtual EndpointSettings GetEndpointSettings(ItemModel source)
    {
      EndpointSettings endpointSettings = new EndpointSettings();
      Endpoint model = this.ConvertReferenceToModel<Endpoint>(source, "EndpointFrom");
      if (model != null)
        endpointSettings.EndpointFrom = model;
      return endpointSettings;
    }
  }
}
