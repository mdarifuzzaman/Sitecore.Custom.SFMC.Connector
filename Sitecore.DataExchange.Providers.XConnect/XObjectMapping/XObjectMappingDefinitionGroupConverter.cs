// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XObjectMapping.XObjectMappingDefinitionGroupConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.XObjectMapping
{
  public class XObjectMappingDefinitionGroupConverter : 
    BaseItemModelConverter<XObjectMappingDefinitionGroup>
  {
    public XObjectMappingDefinitionGroupConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override ConvertResult<XObjectMappingDefinitionGroup> ConvertSupportedItem(
      ItemModel source)
    {
      IEnumerable<XObjectMappingDefinition> models = this.ConvertChildrenToModels<XObjectMappingDefinition>(source);
      XObjectMappingDefinitionGroup mappingDefinitionGroup = new XObjectMappingDefinitionGroup();
      foreach (XObjectMappingDefinition mappingDefinition in models)
        mappingDefinitionGroup.EventMappingDefinitions.Add(mappingDefinition);
      return this.PositiveResult(mappingDefinitionGroup);
    }
  }
}
