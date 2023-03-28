// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps.ResolveXConnectEntityByIdStepConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.PipelineSteps
{
  [SupportedIds(new string[] {"{BC4F1DA8-7EFE-489C-8B2F-B5092B165E99}"})]
  public class ResolveXConnectEntityByIdStepConverter : BaseResolveEntityFromXConnectStepConverter
  {
    public const string FieldNameEntityType = "EntityType";

    public ResolveXConnectEntityByIdStepConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override EntityType GetEntityType(
      ItemModel source,
      PipelineStep pipelineStep)
    {
      return this.GetEnumValueFromReference<EntityType>(source, "EntityType", "ItemName");
    }
  }
}
