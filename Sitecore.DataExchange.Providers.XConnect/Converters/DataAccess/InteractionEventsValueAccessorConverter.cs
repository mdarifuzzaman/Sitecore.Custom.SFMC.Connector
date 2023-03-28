// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.InteractionEventsValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Providers.XConnect.XObjectMapping;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess
{
  [SupportedIds(new string[] {"{60235883-9D10-4ABD-829E-C00BB3DE9806}"})]
  public class InteractionEventsValueAccessorConverter : ValueAccessorConverter
  {
    public const string FieldNameCollectionModelType = "CollectionModelType";
    public const string FieldNameCollectionModel = "CollectionModel";
    public const string FieldNameObjectMappingDefinitionGroups = "ObjectMappingDefinitionGroups";

    public InteractionEventsValueAccessorConverter(IItemModelRepository repository)
      : base(repository)
    {
    }

    protected override IValueWriter GetValueWriter(ItemModel source)
    {
      IEnumerable<XObjectMappingDefinitionGroup> models = this.ConvertReferencesToModels<XObjectMappingDefinitionGroup>(source, "ObjectMappingDefinitionGroups");
      if (models == null)
        return (IValueWriter) null;
      Type typeFromReference = this.GetTypeFromReference(source, "CollectionModel", "CollectionModelType");
      if (typeFromReference == (Type) null)
        return (IValueWriter) null;
      XdbModel xdbModel = typeFromReference.GetXdbModel();
      return xdbModel == null ? (IValueWriter) null : (IValueWriter) new InteractionEventsValueWriter(models, xdbModel);
    }
  }
}
