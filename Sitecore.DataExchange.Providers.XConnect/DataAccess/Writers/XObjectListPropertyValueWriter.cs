// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.XObjectListPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class XObjectListPropertyValueWriter : IValueWriter
  {
    public XObjectListPropertyValueWriter()
    {
    }

    public XObjectListPropertyValueWriter(
      string propertyName,
      IMappingSet mappingSet,
      bool createNewListIfPropertyIsNull = true)
    {
      Condition.Requires<string>(propertyName).IsNotNullOrWhiteSpace();
      Condition.Requires<IMappingSet>(mappingSet).IsNotNull<IMappingSet>();
      this.PropertyName = propertyName;
      this.MappingSet = mappingSet;
      this.CreateNewListIfPropertyIsNull = createNewListIfPropertyIsNull;
    }

    public string PropertyName { get; set; }

    public Type XObjectType { get; set; }

    public IMappingSet MappingSet { get; set; }

    public bool CreateNewListIfPropertyIsNull { get; set; }

    public bool Write(object target, object value, DataAccessContext context)
    {
      if (!(value is IEnumerable enumerable))
        return true;
      if (!(target is XObject target1) || target1.ClrObject == null)
        return false;
      IList collection = this.GetCollection((object) target1, this.CreateNewListIfPropertyIsNull);
      if (this.CreateNewListIfPropertyIsNull && collection == null)
        return false;
      if (collection == null)
        return true;
      ArrayList arrayList = new ArrayList();
      foreach (object obj in enumerable)
      {
        object xobject = this.CreateXObject(this.GetListElementType(collection.GetType()));
        if (xobject == null)
          return false;
        MappingContext context1 = new MappingContext()
        {
          Source = obj,
          Target = xobject
        };
        if (!this.MappingSet.Run(context1) || context1.RunFail.Any<IMapping>())
          return false;
        arrayList.Add(xobject);
      }
      foreach (object obj in arrayList)
        collection.Add(obj);
      return true;
    }

    protected virtual bool CanWrite(object target, object value, DataAccessContext context) => target is XObject;

    protected virtual Type GetListElementType(Type lisType) => lisType == (Type) null || !lisType.IsGenericType || lisType.GenericTypeArguments.Length != 1 ? (Type) null : lisType.GenericTypeArguments[0];

    protected virtual IList GetCollection(object target, bool createIfDoesNotExist)
    {
      if (!(target is XObject xobject1))
        return (IList) null;
      if (xobject1[this.PropertyName] is XCollection source)
        return (IList) source.ToList<object>();
      if (!createIfDoesNotExist)
        return (IList) null;
      object clrObject = xobject1.ClrObject;
      PropertyInfo property = clrObject.GetType().GetProperty(this.PropertyName);
      if (property == (PropertyInfo) null)
        return (IList) null;
      object xobject2 = this.CreateXObject(property.PropertyType);
      if (xobject2 == null)
        return (IList) null;
      property.SetValue(clrObject, xobject2);
      return xobject2 as IList;
    }

    private object CreateXObject(Type type) => Activator.CreateInstance(type);
  }
}
