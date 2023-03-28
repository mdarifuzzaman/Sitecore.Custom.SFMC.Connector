// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Extensions.TypeExtensions
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect.Schema;
using System;
using System.Reflection;

namespace Sitecore.DataExchange.Providers.XConnect.Extensions
{
  public static class TypeExtensions
  {
    public static XdbModel GetXdbModel(this Type collectionModelType)
    {
      if (collectionModelType == (Type) null)
        return (XdbModel) null;
      PropertyInfo property = collectionModelType.GetProperty("Model", BindingFlags.Static | BindingFlags.Public);
      return property == (PropertyInfo) null ? (XdbModel) null : property.GetValue((object) collectionModelType) as XdbModel;
    }
  }
}
