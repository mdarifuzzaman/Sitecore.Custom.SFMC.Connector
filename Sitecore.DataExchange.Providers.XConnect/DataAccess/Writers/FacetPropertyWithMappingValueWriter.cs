// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.FacetPropertyWithMappingValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.Framework.Conditions;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
  public class FacetPropertyWithMappingValueWriter : FacetPropertyValueWriter
  {
    public FacetPropertyWithMappingValueWriter(
      string facetName,
      string propertyName,
      IMappingSet mappingSet)
      : base(facetName, propertyName)
    {
      Condition.Requires<IMappingSet>(mappingSet).IsNotNull<IMappingSet>();
      this.MappingSet = mappingSet;
    }

    public IMappingSet MappingSet { get; set; }

    public override bool Write(object target, object value, DataAccessContext context)
    {
      target = this.GetTarget(target);
      return this.MappingSet.Run(new MappingContext()
      {
        Source = value,
        Target = target
      });
    }

    protected override IValueWriter GetValueWriter(object target) => base.GetValueWriter(target);

    protected virtual object GetTarget(object target)
    {
      IValueReader valueReader = (IValueReader) new PropertyValueReader(this.PropertyName);
      if (target is EntityModel)
        valueReader = (IValueReader) new SequentialValueReader()
        {
          Readers = {
            (IValueReader) new FacetValueReader(this.FacetName),
            valueReader
          }
        };
      ReadResult readResult = valueReader.Read(target, new DataAccessContext());
      return readResult.WasValueRead ? readResult.ReadValue : target;
    }
  }
}
