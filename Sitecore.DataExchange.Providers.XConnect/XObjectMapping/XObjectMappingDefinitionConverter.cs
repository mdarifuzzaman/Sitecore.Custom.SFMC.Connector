// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.XObjectMappingDefinitionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public class XObjectMappingDefinitionConverter : BaseItemModelConverter<XObjectMappingDefinition>
  {
    public const string FieldNameMappingIncludeConditions = "MappingIncludeConditions";
    public const string FieldNameMappingExcludeConditions = "MappingExcludeConditions";
    public const string FieldNameClrType = "ClrType";
    public const string FieldNameMappingSetForProperties = "MappingSetForProperties";

    public XObjectMappingDefinitionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<XObjectMappingDefinition> ConvertSupportedItem(
      ItemModel source)
    {
      return this.PositiveResult(new XObjectMappingDefinition()
      {
        MappingConditionSet = this.GetEventMappingConditionSet(source),
        ClrType = this.GetTypeFromTypeName(source, "ClrType"),
        MappingSetForProperties = this.ConvertReferenceToModel<IMappingSet>(source, "MappingSetForProperties")
      });
    }

    protected virtual IXObjectMappingConditionSet GetEventMappingConditionSet(
      ItemModel source)
    {
      SequentialXObjectMappingConditionSet mappingConditionSet = new SequentialXObjectMappingConditionSet();
      foreach (IXObjectMappingCondition model in this.ConvertReferencesToModels<IXObjectMappingCondition>(source, "MappingIncludeConditions"))
        mappingConditionSet.MappingIncludeConditions.Add(model);
      foreach (IXObjectMappingCondition model in this.ConvertReferencesToModels<IXObjectMappingCondition>(source, "MappingExcludeConditions"))
        mappingConditionSet.MappingExcludeConditions.Add(model);
      return (IXObjectMappingConditionSet) mappingConditionSet;
    }
  }
}
