// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.SubmitObjects.SubmitSingleObjectStepConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.Salesforce.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.Salesforce.SubmitObjects
{
  [SupportedIds(new string[] {"{31A4670C-8E4F-4FFD-98FA-707AC1052747}"})]
  public class SubmitSingleObjectStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameEndpointTo = "EndpointTo";
    public const string FieldNameObjectLocation = "ObjectLocation";
    public const string FieldNameResultObjectLOaction = "ResultObjectLocation";
    public const string FieldNameObjectName = "ObjectName";
    public const string FieldNameSitecoreIdFieldName = "SitecoreIdFieldName";

    public SubmitSingleObjectStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      ObjectLocationSettings objectLocation = this.GetObjectLocation(source);
      EndpointSettings endpointSettings = this.GetEndpointSettings(source);
      SubmitObjectSettings submitObjectSettings = this.GetSubmitObjectSettings(source);
      pipelineStep.AddPlugins((IPlugin) endpointSettings, (IPlugin) objectLocation, (IPlugin) submitObjectSettings);
    }

    protected virtual EndpointSettings GetEndpointSettings(ItemModel source) => new EndpointSettings()
    {
      EndpointTo = this.ConvertReferenceToModel<Endpoint>(source, "EndpointTo")
    };

    protected virtual ObjectLocationSettings GetObjectLocation(
      ItemModel source)
    {
      return new ObjectLocationSettings()
      {
        ObjectLocationId = this.GetGuidValue(source, "ObjectLocation"),
        ResultObjectLocationId = this.GetGuidValue(source, "ResultObjectLocation")
      };
    }

    protected virtual SubmitObjectSettings GetSubmitObjectSettings(
      ItemModel source)
    {
      return new SubmitObjectSettings()
      {
        ObjectName = this.GetStringValue(source, "ObjectName"),
        SitecoreIdFieldName = this.GetStringValue(source, "SitecoreIdFieldName")
      };
    }
  }
}
