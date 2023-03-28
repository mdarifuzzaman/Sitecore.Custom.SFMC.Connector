// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.SubmitXdbReferenceDataBatchStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  [SupportedIds(new string[] {"{30D95294-CD36-4DBF-8C97-C4EB6A0EFE17}"})]
  public class SubmitXdbReferenceDataBatchStepConverter : BasePipelineStepConverter
  {
    public SubmitXdbReferenceDataBatchStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
    {
    }
  }
}
