﻿// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.FacetValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
  public class FacetValueReader : IValueReader
  {
    public FacetValueReader(string facetName) => this.FacetName = facetName;

    public string FacetName { get; protected set; }

    public virtual ReadResult Read(object source, DataAccessContext context) => !this.CanRead(source, context) ? ReadResult.NegativeResult(DateTime.Now) : ReadResult.PositiveResult((object) ((EntityModel) source).GetFacet(this.FacetName), DateTime.Now);

    protected virtual bool CanRead(object source, DataAccessContext context) => !string.IsNullOrWhiteSpace(this.FacetName) && source is EntityModel entityModel && entityModel.HasFacet(this.FacetName);
  }
}
