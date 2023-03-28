// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.FacetPropertyValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using System;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
  public class FacetPropertyValueReader : IValueReader
  {
    public FacetPropertyValueReader(string facetName, string propertyName)
    {
      this.FacetName = facetName;
      this.PropertyName = propertyName;
    }

    public string FacetName { get; protected set; }

    public string PropertyName { get; protected set; }

    public virtual ReadResult Read(object source, DataAccessContext context)
    {
      if (!this.CanRead(source, context))
        return ReadResult.NegativeResult(DateTime.Now);
      switch (source)
      {
        case Facet source1:
          return new PropertyValueReader(this.PropertyName).Read((object) source1, context);
        case EntityModel source2:
          return new SequentialValueReader()
          {
            Readers = {
              (IValueReader) new FacetValueReader(this.FacetName),
              (IValueReader) new PropertyValueReader(this.PropertyName)
            }
          }.Read((object) source2, context);
        default:
          return ReadResult.NegativeResult(DateTime.Now);
      }
    }

    protected virtual bool CanRead(object source, DataAccessContext context)
    {
      switch (source)
      {
        case EntityModel _:
        case Facet _:
          return true;
        default:
          return false;
      }
    }
  }
}
