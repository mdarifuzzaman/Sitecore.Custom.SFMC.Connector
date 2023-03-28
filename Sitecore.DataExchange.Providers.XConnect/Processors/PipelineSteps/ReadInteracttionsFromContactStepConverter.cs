// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ReadInteracttionsFromContactStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class ReadInteracttionsFromContactStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameDoNotConvertToEntityModel = "DoNotConvertToEntityModel";
    public const string FieldNameMaxCount = "MaxCount";
    public const string FieldNameFilter = "Filter";
    public const string FieldNameContactLocation = "ContactLocation";

    public ReadInteracttionsFromContactStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      ReadEntitySettings readEntitySettings = new ReadEntitySettings()
      {
        MaxCount = this.GetIntValue(source, "MaxCount"),
        EntityExpressionBuilder = this.ConvertReferenceToModel<IEntityExpressionBuilder>(source, "Filter")
      };
      XConnectContactIdentifierSettings identifierSettings = new XConnectContactIdentifierSettings()
      {
        ContactLocation = this.GetGuidValue(source, "ContactLocation")
      };
      pipelineStep.AddPlugins((IPlugin) readEntitySettings, (IPlugin) identifierSettings);
    }
  }
}
