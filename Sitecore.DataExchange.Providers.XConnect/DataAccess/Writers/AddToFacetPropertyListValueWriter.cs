// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.AddToFacetPropertyListValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Writers;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class AddToFacetPropertyListValueWriter : FacetPropertyValueWriter
  {
    public AddToFacetPropertyListValueWriter(string facetName, string propertyName)
      : base(facetName, propertyName)
    {
    }

    public override IValueWriter GetPropertyValueWriter(string propName) => (IValueWriter) new AddToPropertyListValueWriter(propName);
  }
}
