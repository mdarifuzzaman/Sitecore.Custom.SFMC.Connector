// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.FacetDictionaryPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class FacetDictionaryPropertyValueWriter : XObjectFacetPropertyValueWriter
  {
    public FacetDictionaryPropertyValueWriter(
      string facetName,
      string propertyName,
      EntityType entityType,
      XdbModel xdbModel,
      IMappingSet mappingSet)
      : base(facetName, propertyName, entityType, xdbModel, mappingSet)
    {
      this.PropertyValueReader = (IValueReader) new Sitecore.DataExchange.DataAccess.Readers.PropertyValueReader(propertyName);
    }

    public IValueReader PropertyValueReader { get; set; }

    protected override bool ApplyMappingSet(
      XdbObjectType xdbObjectType,
      object target,
      object value,
      DataAccessContext context)
    {
      if (!(value is IDictionary dictionary1))
        return false;
      IDictionary dictionary2 = this.GetDictionary(target, value, context);
      if (dictionary2 == null)
        return false;
      Dictionary<object, XObject> dictionary3 = new Dictionary<object, XObject>();
      foreach (object key in (IEnumerable) dictionary1.Keys)
      {
        object obj = dictionary1[key];
        XObject xobject = new XObject(xdbObjectType);
        MappingContext context1 = new MappingContext()
        {
          Source = obj,
          Target = (object) xobject
        };
        if (!this.MappingSet.Run(context1) || context1.RunFail.Any<IMapping>())
          return false;
        dictionary3[key] = xobject;
      }
      foreach (object key in dictionary3.Keys)
      {
        object clrObject = dictionary3[key].ClrObject;
        if (clrObject.GetType() == typeof (ProfileScore))
        {
          Guid profileDefinitionId = ((ProfileScore) clrObject).ProfileDefinitionId;
          dictionary2[(object) profileDefinitionId] = clrObject;
        }
        else
          dictionary2[key] = clrObject;
      }
      return true;
    }

    protected virtual IDictionary GetDictionary(
      object target,
      object value,
      DataAccessContext context)
    {
      if (!(target is Facet source))
        return (IDictionary) null;
      ReadResult readResult = new Sitecore.DataExchange.DataAccess.Readers.PropertyValueReader(this.PropertyName).Read((object) source, context);
      return !readResult.WasValueRead ? (IDictionary) null : readResult.ReadValue as IDictionary;
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
      if (!(readResult.ReadValue is IDictionary))
        return (Type) null;
      Type type = readResult.ReadValue.GetType();
      return !type.IsGenericType ? typeof (object) : ((IEnumerable<Type>) type.GetGenericArguments()).Skip<Type>(1).FirstOrDefault<Type>();
    }
  }
}
