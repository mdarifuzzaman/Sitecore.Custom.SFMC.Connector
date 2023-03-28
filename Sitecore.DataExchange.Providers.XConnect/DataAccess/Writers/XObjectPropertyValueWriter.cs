// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.XObjectPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class XObjectPropertyValueWriter : IValueWriter
  {
    public XObjectPropertyValueWriter(
      string propertyName,
      EntityType entityType,
      XdbModel xdbModel,
      IMappingSet mappingSet)
    {
      Condition.Requires<string>(propertyName).IsNotNullOrWhiteSpace();
      Condition.Requires<XdbModel>(xdbModel).IsNotNull<XdbModel>();
      Condition.Requires<IMappingSet>(mappingSet).IsNotNull<IMappingSet>();
      this.PropertyName = propertyName;
      this.EntityType = entityType;
      this.XdbModel = xdbModel;
      this.MappingSet = mappingSet;
      this.PropertyValueWriter = (IValueWriter) new Sitecore.DataExchange.DataAccess.Writers.PropertyValueWriter(propertyName);
      this.ReflectionUtil = (IReflectionUtil) Sitecore.DataExchange.DataAccess.Reflection.ReflectionUtil.Instance;
    }

    public string PropertyName { get; private set; }

    public EntityType EntityType { get; private set; }

    public XdbModel XdbModel { get; private set; }

    public IMappingSet MappingSet { get; set; }

    public IReflectionUtil ReflectionUtil { get; set; }

    protected IValueWriter PropertyValueWriter { get; set; }

    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      Type clrTypeForXobject = this.GetClrTypeForXObject(target, value, context);
      XdbObjectType type;
      return !(clrTypeForXobject == (Type) null) && this.XdbModel.TryGetType<XdbObjectType>(clrTypeForXobject, out type) && this.ApplyMappingSet(type, target, value, context);
    }

    protected virtual bool ApplyMappingSet(
      XdbObjectType xdbObjectType,
      object target,
      object value,
      DataAccessContext context)
    {
      XObject xobject = new XObject(xdbObjectType);
      return this.MappingSet.Run(new MappingContext()
      {
        Source = value,
        Target = (object) xobject
      }) && this.PropertyValueWriter != null && this.PropertyValueWriter.Write(target, xobject.ClrObject, context);
    }

    protected virtual Type GetClrTypeForXObject(
      object target,
      object value,
      DataAccessContext context)
    {
      PropertyInfo property = this.ReflectionUtil.GetProperty(this.PropertyName, target);
      return property == (PropertyInfo) null ? (Type) null : property.PropertyType;
    }
  }
}
