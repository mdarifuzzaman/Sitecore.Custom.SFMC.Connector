// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ReferenceDataHelper
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Xdb.ReferenceData.Core;
using Sitecore.Xdb.ReferenceData.Core.Collections;
using Sitecore.Xdb.ReferenceData.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  public class ReferenceDataHelper
  {
    public ReferenceDataHelper() => this.ReflectionUtil = (IReflectionUtil) Sitecore.DataExchange.DataAccess.Reflection.ReflectionUtil.Instance;

    public IReflectionUtil ReflectionUtil { get; set; }

    public virtual BaseDefinition GetDefinition(
      ReferenceDataHelperContext context,
      ILogger logger)
    {
      DefinitionTypeKey typeKey = context.Client.EnsureDefinitionType(context.DefinitionTypeName);
      Type[] genericParameterTypes = new Type[2]
      {
        context.CommonDataType,
        context.CultureDataType
      };
      object[] objArray = new object[2]
      {
        (object) new DefinitionCriteria(context.Moniker, typeKey),
        (object) false
      };
      Type[] array = ((IEnumerable<object>) objArray).Select<object, Type>((Func<object, Type>) (p => p.GetType())).ToArray<Type>();
      MethodInfo method = this.ReflectionUtil.GetMethod(nameof (GetDefinition), typeof (IReadOnlyReferenceDataClient), genericParameterTypes, array);
      if (method == (MethodInfo) null)
        return (BaseDefinition) null;
      if (!(method.Invoke((object) context.Client, objArray) is BaseDefinition definition))
        return (BaseDefinition) null;
      return !this.InitializePropertiesIfNeeded(definition, context, logger) ? (BaseDefinition) null : definition;
    }

    public virtual SaveDefinitionsBatchResult SubmitDefinitions(
      IList<BaseDefinition> definitions,
      ReferenceDataHelperContext context,
      ILogger logger)
    {
      Type[] genericParameterTypes = new Type[2]
      {
        context.CommonDataType,
        context.CultureDataType
      };
      object genericTypeDefinition = this.ReflectionUtil.CreateObjectFromGenericTypeDefinition(typeof (DefinitionCollection<,>), genericParameterTypes);
      if (genericTypeDefinition == null)
        return (SaveDefinitionsBatchResult) null;
      Type[] args = new Type[1]
      {
        typeof (Definition<,>).MakeGenericType(context.CommonDataType, context.CultureDataType)
      };
      MethodInfo method1 = this.ReflectionUtil.GetMethod("Add", genericTypeDefinition, (ICollection<object>) args);
      if (method1 == (MethodInfo) null)
        return (SaveDefinitionsBatchResult) null;
      foreach (BaseDefinition definition in (IEnumerable<BaseDefinition>) definitions)
        method1.Invoke(genericTypeDefinition, new object[1]
        {
          (object) definition
        });
      object[] objArray = new object[1]
      {
        genericTypeDefinition
      };
      Type[] array = ((IEnumerable<object>) objArray).Select<object, Type>((Func<object, Type>) (p => p.GetType())).ToArray<Type>();
      MethodInfo method2 = this.ReflectionUtil.GetMethod("Save", typeof (IReferenceDataClient), genericParameterTypes, array);
      return method2 == (MethodInfo) null ? (SaveDefinitionsBatchResult) null : method2.Invoke((object) context.Client, objArray) as SaveDefinitionsBatchResult;
    }

    public virtual bool InitializePropertiesIfNeeded(
      BaseDefinition definition,
      ReferenceDataHelperContext context,
      ILogger logger)
    {
      if (context.InitializeCommonData && !this.InitializePropertyIfNeeded("CommonData", definition, context.CommonDataType) || context.InitializeCultureData && !this.InitializePropertyIfNeeded("CultureData", definition, context.CommonDataType))
        return false;
      if (context.ActivateDefinition)
        definition.IsActive = true;
      return true;
    }

    protected virtual bool InitializePropertyIfNeeded(
      string propertyName,
      BaseDefinition definition,
      Type propertyValueType)
    {
      PropertyInfo property = this.ReflectionUtil.GetProperty(propertyName, (object) definition);
      if (property == (PropertyInfo) null)
        return false;
      if (property.GetValue((object) definition) != null)
        return true;
      object instance = Activator.CreateInstance(propertyValueType);
      property.SetValue((object) definition, instance);
      return true;
    }
  }
}
