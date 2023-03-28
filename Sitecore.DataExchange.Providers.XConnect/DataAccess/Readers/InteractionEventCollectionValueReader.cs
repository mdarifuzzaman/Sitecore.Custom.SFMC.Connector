// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.InteractionEventCollectionValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
  public class InteractionEventCollectionValueReader : IValueReader
  {
    public virtual ReadResult Read(object source, DataAccessContext context) => !this.CanRead(source, context) ? ReadResult.NegativeResult(DateTime.Now) : ReadResult.PositiveResult(source is InteractionModel interactionModel ? (object) interactionModel.Events : (object) (List<Event>) null, DateTime.Now);

    protected virtual bool CanRead(object source, DataAccessContext context) => source is InteractionModel;
  }
}
