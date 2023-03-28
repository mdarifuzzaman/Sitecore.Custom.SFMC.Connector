// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.FacetListPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class FacetListPropertyValueWriter : XObjectFacetPropertyValueWriter
  {
    public FacetListPropertyValueWriter(
      string facetName,
      string propertyName,
      EntityType entityType,
      XdbModel xdbModel,
      IMappingSet mappingSet)
      : base(facetName, propertyName, entityType, xdbModel, mappingSet)
    {
      this.PropertyValueReader = (IValueReader) new FacetPropertyValueReader(facetName, propertyName);
    }

    public IValueReader PropertyValueReader { get; set; }

    protected override bool ApplyMappingSet(
      XdbObjectType xdbObjectType,
      object target,
      object value,
      DataAccessContext context)
    {
      if (!(value is ICollection collection))
        return false;
      IList list = this.GetList(target, value, context);
      if (list == null)
        return false;
      List<XObject> xobjectList = new List<XObject>();
      foreach (object obj in (IEnumerable) collection)
      {
        XObject xobject = new XObject(xdbObjectType);
        MappingContext context1 = new MappingContext()
        {
          Source = obj,
          Target = (object) xobject
        };
        if (!this.MappingSet.Run(context1) || context1.RunFail.Any<IMapping>())
          return false;
        xobjectList.Add(xobject);
      }
      foreach (XObject xobject in xobjectList)
        list.Add(xobject.ClrObject);
      return true;
    }

    protected virtual IList GetList(object target, object value, DataAccessContext context)
    {
      if (!(target is Facet source))
        return (IList) null;
      ReadResult readResult = new Sitecore.DataExchange.DataAccess.Readers.PropertyValueReader(this.PropertyName).Read((object) source, context);
      return !readResult.WasValueRead ? (IList) null : readResult.ReadValue as IList;
    }

    protected override Type GetClrTypeForXObject(
      object target,
      object value,
      DataAccessContext context)
    {
      if (target == null)
        return (Type) null;
      ReadResult readResult = this.PropertyValueReader.Read(target, context);
      if (!readResult.WasValueRead || readResult.ReadValue == null)
        return (Type) null;
      if (!(readResult.ReadValue is IList))
        return (Type) null;
      Type type = readResult.ReadValue.GetType();
      return !type.IsGenericType ? typeof (object) : ((IEnumerable<Type>) type.GetGenericArguments()).FirstOrDefault<Type>();
    }
  }
}
