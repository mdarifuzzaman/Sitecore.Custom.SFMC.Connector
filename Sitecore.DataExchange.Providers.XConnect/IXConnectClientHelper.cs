// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.IXConnectClientHelper
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace Sitecore.DataExchange.Providers.XConnect
{
  public interface IXConnectClientHelper
  {
    Contact ToContact(ContactModel contactModel);

    ContactModel ToContactModel(Contact contact);

    DeviceProfile ToDeviceProfile(DeviceProfileModel deviceProfileModel);

    DeviceProfileModel ToDeviceProfileModel(DeviceProfile deviceProfile);

    IXdbContext ToXConnectClient(XConnectClientSettings clientSettings);

    Facet ToFacet(string facetName, XdbModel xdbModel, EntityType entityType);

    Interaction ToInteraction(InteractionModel interactionModel);

    InteractionModel ToInteractionModel(Interaction interaction);

    IEntityReference ToEntityReference(
      EntityType entityType,
      EntityModel entityModel);
  }
}
