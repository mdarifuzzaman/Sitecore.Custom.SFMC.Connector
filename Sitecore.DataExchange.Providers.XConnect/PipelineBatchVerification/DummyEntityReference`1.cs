// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification.DummyEntityReference`1
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification
{
  public class DummyEntityReference<TEntity> : 
    IEntityReference<TEntity>,
    IEntityReference,
    IXdbReference
    where TEntity : Entity
  {
    public DummyEntityReference(Guid? id)
    {
      this.EntityType = TypeHelpers.GetEntityType<TEntity>();
      this.Id = id;
    }

    public EntityType EntityType { get; private set; }

    public Guid? Id { get; private set; }
  }
}
