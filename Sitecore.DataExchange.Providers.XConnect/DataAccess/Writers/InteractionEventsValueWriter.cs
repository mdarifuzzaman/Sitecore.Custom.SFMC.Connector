// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.InteractionEventsValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Providers.XConnect.XObjectMapping;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class InteractionEventsValueWriter : IValueWriter
  {
    public InteractionEventsValueWriter(
      IEnumerable<XObjectMappingDefinitionGroup> groups,
      XdbModel xdbModel)
    {
      if (groups == null)
        throw new ArgumentNullException(nameof (groups));
      if (xdbModel == null)
        throw new ArgumentNullException(nameof (xdbModel));
      this.EventMappingDefinitionGroups = groups;
      this.XdbModel = xdbModel;
    }

    public XdbModel XdbModel { get; private set; }

    public IEnumerable<XObjectMappingDefinitionGroup> EventMappingDefinitionGroups { get; private set; }

    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      if (target == null || !(value is IEnumerable eventDataObjects))
        return false;
      switch (target)
      {
        case InteractionModel interactionModel:
          return this.WritePageViewEvents(interactionModel, eventDataObjects, context);
        case XObject parentEvent:
          return this.WriteEvents(eventDataObjects, parentEvent, context);
        default:
          return false;
      }
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

    protected virtual XObject CreateEventXObject(XdbObjectType type)
    {
      XObject eventXobject = new XObject(type);
      if (eventXobject["Id"] == null || eventXobject["Id"].Equals((object) new Guid()))
        eventXobject["Id"] = (object) Guid.NewGuid();
      return eventXobject;
    }

    protected virtual bool WritePageViewEvents(
      InteractionModel interactionModel,
      IEnumerable eventDataObjects,
      DataAccessContext context)
    {
      List<XObject> xobjectList1 = new List<XObject>();
      foreach (object eventDataObject in eventDataObjects)
      {
        XObjectMappingDefinition mappingDefinition = this.GetFirstMatchingEventMappingDefinition(eventDataObject);
        XdbObjectType type;
        if (mappingDefinition != null && !(mappingDefinition.ClrType == (Type) null) && this.XdbModel.TryGetType<XdbObjectType>(mappingDefinition.ClrType, out type))
        {
          XObject eventXobject = this.CreateEventXObject(type);
          MappingContext context1 = new MappingContext()
          {
            Source = eventDataObject,
            Target = (object) eventXobject
          };
          List<XObject> xobjectList2 = new List<XObject>();
          context1.Plugins.Add((IDataAccessPlugin) new MappingEventSettings()
          {
            Events = xobjectList2
          });
          if (!mappingDefinition.MappingSetForProperties.Run(context1) || context1.RunFail.Any<IMapping>())
            return false;
          this.SetEventParentTimestampIfDoesNotExist(xobjectList2, (object) this.GetTimestamp(eventXobject));
          this.SetEventParentId(xobjectList2, (object) this.GetEventId(eventXobject));
          xobjectList1.Add(eventXobject);
          xobjectList1.AddRange((IEnumerable<XObject>) xobjectList2);
        }
      }
      if (interactionModel.Events == null)
        interactionModel.Events = new List<Event>();
      foreach (XObject xobject in xobjectList1)
        interactionModel.Events.Add(xobject.ClrObject as Event);
      return true;
    }

    protected virtual bool WriteEvents(
      IEnumerable eventDataObjects,
      XObject parentEvent,
      DataAccessContext context)
    {
      foreach (object eventDataObject in eventDataObjects)
      {
        XObjectMappingDefinition mappingDefinition = this.GetFirstMatchingEventMappingDefinition(eventDataObject);
        XdbObjectType type;
        if (mappingDefinition != null && !(mappingDefinition.ClrType == (Type) null) && this.XdbModel.TryGetType<XdbObjectType>(mappingDefinition.ClrType, out type))
        {
          XObject @event = new XObject(type);
          MappingContext context1 = new MappingContext()
          {
            Source = eventDataObject,
            Target = (object) @event
          };
          if (!mappingDefinition.MappingSetForProperties.Run(context1) || context1.RunFail.Any<IMapping>())
            return false;
          this.SetEventParentId(@event, this.GetEventId(parentEvent));
          this.SaveEventToContext(@event, context);
        }
      }
      return true;
    }

    protected virtual void SaveEventToContext(XObject @event, DataAccessContext context)
    {
      if (@event == null || context == null)
        return;
      MappingContextSettings plugin1 = context.GetPlugin<MappingContextSettings>();
      if (plugin1 == null || plugin1.MappingContext == null)
        return;
      MappingEventSettings plugin2 = plugin1.MappingContext.GetPlugin<MappingEventSettings>();
      if (plugin2 == null || plugin2.Events == null)
        return;
      plugin2.Events.Add(@event);
    }

    private void SetEventParentId(XObject @event, Guid? parentId)
    {
      if (!parentId.HasValue || parentId.Value == Guid.Empty)
        return;
      @event["ParentEventId"] = (object) parentId.Value;
    }

    private void SetEventParentTimestampIfDoesNotExist(XObject @event, DateTime? parentDateTime)
    {
      DateTime? timestamp = this.GetTimestamp(@event);
      if (timestamp.HasValue && timestamp.Value != DateTime.MinValue || !parentDateTime.HasValue)
        return;
      @event["Timestamp"] = (object) parentDateTime.Value;
    }

    private void SetEventParentId(XObject @event, object parentId)
    {
      Guid? objectAsGuid = this.GetObjectAsGuid(parentId);
      this.SetEventParentId(@event, objectAsGuid);
    }

    private void SetEventParentId(List<XObject> events, object parentId)
    {
      Guid? objectAsGuid = this.GetObjectAsGuid(parentId);
      foreach (XObject @event in events)
        this.SetEventParentId(@event, objectAsGuid);
    }

    private void SetEventParentTimestampIfDoesNotExist(List<XObject> events, object parentId)
    {
      DateTime? objectAsDateTime = this.GetObjectAsDateTime(parentId);
      foreach (XObject @event in events)
        this.SetEventParentTimestampIfDoesNotExist(@event, objectAsDateTime);
    }

    private Guid? GetObjectAsGuid(object obj)
    {
      if (!(obj is Guid guid))
        return new Guid?();
      return guid == Guid.Empty ? new Guid?() : new Guid?(guid);
    }

    private DateTime? GetObjectAsDateTime(object obj) => !(obj is DateTime dateTime) ? new DateTime?() : new DateTime?(dateTime);

    private Guid? GetEventId(XObject obj)
    {
      if (obj != null)
      {
        object obj1 = obj["Id"];
        if (obj1 != null)
          return this.GetObjectAsGuid(obj1);
      }
      return new Guid?();
    }

    private DateTime? GetTimestamp(XObject obj)
    {
      if (obj != null)
      {
        object obj1 = obj["Timestamp"];
        if (obj1 != null)
          return this.GetObjectAsDateTime(obj1);
      }
      return new DateTime?();
    }
  }
}
