// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.DataAccess.InteractionEventValueAccessorConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
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
  [SupportedIds(new string[] {"{645BCEBC-BEE5-4DB5-9C68-8DFF8D298513}"})]
  public class InteractionEventValueAccessorConverter : InteractionEventsValueAccessorConverter
  {
    public InteractionEventValueAccessorConverter(IItemModelRepository repository)
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
      return xdbModel == null ? (IValueWriter) null : (IValueWriter) new InteractionEventValueWriter(models, xdbModel);
    }
  }
}
