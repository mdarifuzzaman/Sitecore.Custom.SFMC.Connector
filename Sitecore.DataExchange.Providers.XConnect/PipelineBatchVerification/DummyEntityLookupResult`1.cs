// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification.DummyEntityLookupResult`1
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification
{
  public class DummyEntityLookupResult<TEntity> : IEntityLookupResult<TEntity> where TEntity : Sitecore.XConnect.Entity
  {
    public DummyEntityLookupResult(IEntityReference<TEntity> reference) => this.Reference = reference;

    public DummyEntityLookupResult(TEntity entity)
    {
      this.Entity = entity;
      this.Exists = true;
      this.Reference = (IEntityReference<TEntity>) new DummyEntityReference<TEntity>(entity.Id);
    }

    public TEntity Entity { get; private set; }

    public Exception Exception { get; set; }

    public bool Exists { get; set; }

    public IEntityReference<TEntity> Reference { get; private set; }
  }
}
