// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ReadObjectsByIdentifierStepConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  [SupportedIds(new string[] {"{1482A875-468B-41FD-BEB7-914F6F520FB7}"})]
  public class ReadObjectsByIdentifierStepConverter : ReadObjectsStepConverter
  {
    public ReadObjectsByIdentifierStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      base.AddPlugins(source, pipelineStep);
      this.AddResolveIdentifierPlugin(source, pipelineStep);
    }

    protected virtual void AddResolveIdentifierPlugin(ItemModel source, PipelineStep pipelineStep)
    {
      IValueAccessor model1 = this.ConvertReferenceToModel<IValueAccessor>(source, "IdentifierValueAccessor");
      Guid guidValue = this.GetGuidValue(source, "IdentifierObjectLocation");
      IValueReader model2 = this.ConvertReferenceToModel<IValueReader>(source, "ValueReaderToConvertIdentifierValueForComparison");
      ResolveIdentifierSettings identifierSettings = new ResolveIdentifierSettings()
      {
        IdentifierObjectLocation = guidValue,
        IdentifierValueAccessor = model1,
        ValueReaderToConvertIdentifierValueForComparison = model2
      };
      pipelineStep.AddPlugins((IPlugin) identifierSettings);
    }
  }
}
