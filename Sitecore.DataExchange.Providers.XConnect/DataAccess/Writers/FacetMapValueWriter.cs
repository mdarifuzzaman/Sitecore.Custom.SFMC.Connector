// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers.FacetMapValueWriter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Extensions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.Framework.Conditions;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Writers
{
    public class FacetMapValueWriter : IValueWriter
    {
        public FacetMapValueWriter(IMappingSet mappingSet, XdbFacetDefinition facet)
        {
            Condition.Requires<IMappingSet>(mappingSet).IsNotNull<IMappingSet>();
            Condition.Requires<XdbFacetDefinition>(facet).IsNotNull<XdbFacetDefinition>();
            this.MappingSet = mappingSet;
            this.FacetDefinition = facet;
        }

        public IMappingSet MappingSet { get; set; }

        public XdbFacetDefinition FacetDefinition { get; set; }

        public virtual bool Write(object target, object value, DataAccessContext context)
        {
            if (!this.CanWrite(target, value, context))
                return false;
            EntityModel entityModel = null;
            if (target is EntityModel)
            {
                entityModel = target as EntityModel;
            }
            Facet facet = (target is EntityModel ? entityModel.GetFacet(this.FacetDefinition.Name) : (Facet)null) ?? Facet.CreateFromXdbType(this.FacetDefinition.FacetType);
            if (facet == null)
            {
                Type type = Type.GetType(this.FacetDefinition.FacetType.FullName + ", " + this.FacetDefinition.FacetType.Namespace);
                if (type == (Type)null)
                    return false;
                facet = (Facet)Activator.CreateInstance(type);
                if (facet == null)
                    return false;
            }
            MappingContext context1 = new MappingContext()
            {
                Source = value,
                Target = (object)facet
            };

            if (!this.MappingSet.Run(context1))
            {
                if (entityModel.HasFacet(this.FacetDefinition.Name))
                    entityModel.Facets.Remove(this.FacetDefinition.Name);
                return false;
            }
            if (context1.RunSuccess.Any<IMapping>() && !entityModel.HasFacet(this.FacetDefinition.Name))
                entityModel.SetFacet(this.FacetDefinition.Name, facet);
            if (!context1.RunSuccess.Any<IMapping>() && entityModel.HasFacet(this.FacetDefinition.Name))
                entityModel.Facets.Remove(this.FacetDefinition.Name);
            return true;
        }

        protected virtual bool CanWrite(object target, object value, DataAccessContext context) => target is EntityModel;
    }
}
