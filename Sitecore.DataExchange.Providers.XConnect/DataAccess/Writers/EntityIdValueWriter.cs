// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.EntityIdValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Models;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class EntityIdValueWriter : IValueWriter
  {
    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      if (!this.CanWrite(target, value, context) || !(target is EntityModel entityModel))
        return false;
      if (value is Guid guid)
      {
        entityModel.Id = guid;
        return true;
      }
      Guid result;
      if (!Guid.TryParse(value.ToString(), out result))
        return false;
      entityModel.Id = result;
      return true;
    }

    protected virtual bool CanWrite(object target, object value, DataAccessContext context) => target is EntityModel && (value is Guid || Guid.TryParse((string) value, out Guid _));
  }
}
