// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.FacetPropertyValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class FacetPropertyValueWriter : IValueWriter
  {
    public FacetPropertyValueWriter(string facetName, string propertyName)
    {
      this.FacetName = facetName;
      this.PropertyName = propertyName;
    }

    public string FacetName { get; protected set; }

    public string PropertyName { get; protected set; }

    public virtual bool Write(object target, object value, DataAccessContext context)
    {
      if (!this.CanWrite(target, value, context))
        return false;
      IValueWriter valueWriter = this.GetValueWriter(target);
      return valueWriter != null && valueWriter.Write(target, value, context);
    }

    public virtual IValueWriter GetPropertyValueWriter(string propName) => (IValueWriter) new PropertyValueWriter(propName);

    protected virtual bool CanWrite(object target, object value, DataAccessContext context)
    {
      switch (target)
      {
        case EntityModel _:
        case Facet _:
          return true;
        default:
          return false;
      }
    }

    protected virtual IValueWriter GetValueWriter(object target)
    {
      switch (target)
      {
        case Facet _:
          return this.GetPropertyValueWriter(this.PropertyName);
        case EntityModel _:
          return (IValueWriter) new TargetReaderValueWriter()
          {
            ValueAccessor = (IValueAccessor) new ValueAccessor()
            {
              ValueReader = (IValueReader) new FacetValueReader(this.FacetName),
              ValueWriter = this.GetPropertyValueWriter(this.PropertyName)
            }
          };
        default:
          return (IValueWriter) null;
      }
    }
  }
}
