// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.ReadInteractionsFromContactStepConverter
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

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{F4B81859-2CFA-45F4-AFC1-1A0499CACEAB}"})]
  public class ReadInteractionsFromContactStepConverter : BasePipelineStepConverter
  {
    public const string FieldNameContactLocation = "ContactLocation";
    public const string FieldNameFilter = "Filter";
    public const string FieldNameMaxCount = "MaxCount";
    public const string FieldNameDoNotConvertToEntityModel = "DoNotConvertToEntityModel";

    public ReadInteractionsFromContactStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
      XConnectContactIdentifierSettings identifierSettings = new XConnectContactIdentifierSettings()
      {
        ContactLocation = this.GetGuidValue(source, "ContactLocation")
      };
      ReadEntitySettings readEntitySettings = new ReadEntitySettings()
      {
        MaxCount = this.GetIntValue(source, "MaxCount"),
        EntityExpressionBuilder = this.ConvertReferenceToModel<IEntityExpressionBuilder>(source, "Filter")
      };
      pipelineStep.AddPlugins((IPlugin) identifierSettings, (IPlugin) readEntitySettings);
    }
  }
}
