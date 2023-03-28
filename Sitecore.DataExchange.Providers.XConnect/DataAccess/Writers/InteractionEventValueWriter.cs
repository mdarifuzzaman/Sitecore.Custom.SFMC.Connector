// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.InteractionEventValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.XObjectMapping;
using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class InteractionEventValueWriter : IValueWriter
  {
    public InteractionEventValueWriter(
      IEnumerable<XObjectMappingDefinitionGroup> groups,
      XdbModel xdbModel)
    {
      Condition.Requires<IEnumerable<XObjectMappingDefinitionGroup>>(groups).IsNotNull<IEnumerable<XObjectMappingDefinitionGroup>>();
      Condition.Requires<XdbModel>(xdbModel).IsNotNull<XdbModel>();
      this.EventMappingDefinitionGroups = groups;
      this.XdbModel = xdbModel;
    }

    public XdbModel XdbModel { get; private set; }

    public IEnumerable<XObjectMappingDefinitionGroup> EventMappingDefinitionGroups { get; private set; }

    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      if (!(target is InteractionModel interactionModel))
        return false;
      XObjectMappingDefinition mappingDefinition = this.GetFirstMatchingEventMappingDefinition(value);
      XdbObjectType type;
      if (mappingDefinition == null || mappingDefinition.ClrType == (Type) null || !this.XdbModel.TryGetType<XdbObjectType>(mappingDefinition.ClrType, out type))
        return false;
      XObject eventXobject = this.CreateEventXObject(type);
      MappingContext context1 = new MappingContext()
      {
        Source = value,
        Target = (object) eventXobject
      };
      if (!mappingDefinition.MappingSetForProperties.Run(context1) || context1.RunFail.Any<IMapping>())
        return false;
      if (interactionModel.Events == null)
        interactionModel.Events = new List<Event>();
      interactionModel.Events.Add(eventXobject.ClrObject as Event);
      return true;
    }

    protected virtual XObject CreateEventXObject(XdbObjectType type)
    {
      XObject eventXobject = new XObject(type);
      if (eventXobject["Id"] == null || eventXobject["Id"].Equals((object) new Guid()))
        eventXobject["Id"] = (object) Guid.NewGuid();
      return eventXobject;
    }

    protected virtual XObjectMappingDefinition GetFirstMatchingEventMappingDefinition(
      object sourceEvent)
    {
      foreach (XObjectMappingDefinitionGroup mappingDefinitionGroup in this.EventMappingDefinitionGroups)
      {
        XObjectMappingDefinition mappingDefinition = mappingDefinitionGroup.GetFirstMatchingEventMappingDefinition(sourceEvent);
        if (mappingDefinition != null)
          return mappingDefinition;
      }
      return (XObjectMappingDefinition) null;
    }
  }
}
