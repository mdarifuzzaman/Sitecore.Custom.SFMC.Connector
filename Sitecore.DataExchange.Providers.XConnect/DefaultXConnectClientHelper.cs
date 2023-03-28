// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DefaultXConnectClientHelper
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;

namespace Sitecore.DataExchange.Providers.XConnect
{
  public class DefaultXConnectClientHelper : IXConnectClientHelper
  {
    private IXConnectClientHelperFactory factory;

    public DefaultXConnectClientHelper()
      : this((IXConnectClientHelperFactory) new DefaultXConnectClientHelperFactory())
    {
    }

    public DefaultXConnectClientHelper(IXConnectClientHelperFactory clientHelperFactory)
    {
      if (clientHelperFactory == null)
      {
        Context.Logger.Error("Client helper factory is null. (factory: IXConnectClientHelperFactory)");
        throw new ArgumentNullException();
      }
      this.factory = clientHelperFactory;
    }

    public virtual IXdbContext ToXConnectClient(XConnectClientSettings settings) => this.factory.ToXConnectClient(settings);

    public Facet ToFacet(string facetName, XdbModel xdbModel, EntityType entityType) => this.factory.ToFacet(facetName, xdbModel, entityType);

    public Interaction ToInteraction(InteractionModel interactionModel) => this.factory.ToInteraction(interactionModel);

    public InteractionModel ToInteractionModel(Interaction interaction) => this.factory.ToInteractionModel(interaction);

    public virtual Contact ToContact(ContactModel contactModel) => this.factory.ToContact(contactModel);

    public virtual ContactModel ToContactModel(Contact contact) => this.factory.ToContactModel(contact);

    public virtual DeviceProfile ToDeviceProfile(DeviceProfileModel deviceProfileModel) => this.factory.ToDeviceProfile(deviceProfileModel);

    public virtual DeviceProfileModel ToDeviceProfileModel(
      DeviceProfile deviceProfile)
    {
      return this.factory.ToDeviceProfileModel(deviceProfile);
    }

    public virtual IEntityReference ToEntityReference(
      EntityType entityType,
      EntityModel entityModel)
    {
      IEntityReference entityReference = (IEntityReference) null;
      if (entityModel != null)
      {
        switch (entityType)
        {
          case EntityType.Contact:
            entityReference = this.GetEntityReferenceForContact(entityModel);
            break;
          case EntityType.Interaction:
            entityReference = (IEntityReference) this.ToInteraction(entityModel as InteractionModel);
            break;
        }
      }
      return entityReference;
    }

    protected virtual IEntityReference GetEntityReferenceForContact(
      EntityModel entityModel)
    {
      return entityModel.Id != Guid.Empty ? (IEntityReference) new ContactReference(entityModel.Id) : (IEntityReference) this.ToContact(entityModel as ContactModel);
    }
  }
}
