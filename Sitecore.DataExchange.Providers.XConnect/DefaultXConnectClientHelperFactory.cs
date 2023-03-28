// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DefaultXConnectClientHelperFactory
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
  public class DefaultXConnectClientHelperFactory : IXConnectClientHelperFactory
  {
    private static IConverter<XConnectClientSettings, IXdbContext> _xconnectClientConverter = (IConverter<XConnectClientSettings, IXdbContext>) new Sitecore.DataExchange.Providers.XConnect.Client.XConnectClientConverter();
    private static IConverter<ContactModel, Contact> _contactConverter = (IConverter<ContactModel, Contact>) new Sitecore.DataExchange.Providers.XConnect.Converters.Models.ContactConverter();
    private static IConverter<Contact, ContactModel> _contactModelConverter = (IConverter<Contact, ContactModel>) new Sitecore.DataExchange.Providers.XConnect.Converters.Models.ContactModelConverter();
    private static IConverter<DeviceProfileModel, DeviceProfile> _deviceProfileConverter = (IConverter<DeviceProfileModel, DeviceProfile>) new Sitecore.DataExchange.Providers.XConnect.Converters.Models.DeviceProfileConverter();
    private static IConverter<DeviceProfile, DeviceProfileModel> _deviceProfileModelConverter = (IConverter<DeviceProfile, DeviceProfileModel>) new Sitecore.DataExchange.Providers.XConnect.Converters.Models.DeviceProfileModelConverter();
    private static IConverter<InteractionModel, Interaction> _interactionConverter = (IConverter<InteractionModel, Interaction>) new Sitecore.DataExchange.Providers.XConnect.Converters.Models.InteractionConverter();
    private static IConverter<Interaction, InteractionModel> _interactionModelConverter = (IConverter<Interaction, InteractionModel>) new Sitecore.DataExchange.Providers.XConnect.Converters.Models.InteractionModelConverter();

    public DefaultXConnectClientHelperFactory()
    {
      this.XConnectClientConverter = DefaultXConnectClientHelperFactory._xconnectClientConverter;
      this.ContactConverter = DefaultXConnectClientHelperFactory._contactConverter;
      this.ContactModelConverter = DefaultXConnectClientHelperFactory._contactModelConverter;
      this.DeviceProfileConverter = DefaultXConnectClientHelperFactory._deviceProfileConverter;
      this.DeviceProfileModelConverter = DefaultXConnectClientHelperFactory._deviceProfileModelConverter;
      this.InteractionConverter = DefaultXConnectClientHelperFactory._interactionConverter;
      this.InteractionModelConverter = DefaultXConnectClientHelperFactory._interactionModelConverter;
    }

    public IConverter<XConnectClientSettings, IXdbContext> XConnectClientConverter { get; set; }

    public IConverter<ContactModel, Contact> ContactConverter { get; set; }

    public IConverter<Contact, ContactModel> ContactModelConverter { get; set; }

    public IConverter<DeviceProfileModel, DeviceProfile> DeviceProfileConverter { get; set; }

    public IConverter<DeviceProfile, DeviceProfileModel> DeviceProfileModelConverter { get; set; }

    public IConverter<InteractionModel, Interaction> InteractionConverter { get; set; }

    public IConverter<Interaction, InteractionModel> InteractionModelConverter { get; set; }

    public virtual IXdbContext ToXConnectClient(XConnectClientSettings clientSettings)
    {
      ConvertResult<IXdbContext> convertResult = clientSettings != null ? this.XConnectClientConverter.Convert(clientSettings) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (IXdbContext) null : convertResult.ConvertedValue;
    }

    public virtual Contact ToContact(ContactModel contactModel)
    {
      ConvertResult<Contact> convertResult = contactModel != null ? this.ContactConverter.Convert(contactModel) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (Contact) null : convertResult.ConvertedValue;
    }

    public virtual ContactModel ToContactModel(Contact contact)
    {
      ConvertResult<ContactModel> convertResult = contact != null ? this.ContactModelConverter.Convert(contact) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (ContactModel) null : convertResult.ConvertedValue;
    }

    public virtual DeviceProfile ToDeviceProfile(DeviceProfileModel deviceProfileModel)
    {
      ConvertResult<DeviceProfile> convertResult = deviceProfileModel != null ? this.DeviceProfileConverter.Convert(deviceProfileModel) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (DeviceProfile) null : convertResult.ConvertedValue;
    }

    public virtual DeviceProfileModel ToDeviceProfileModel(
      DeviceProfile deviceProfile)
    {
      ConvertResult<DeviceProfileModel> convertResult = deviceProfile != null ? this.DeviceProfileModelConverter.Convert(deviceProfile) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (DeviceProfileModel) null : convertResult.ConvertedValue;
    }

    public Facet ToFacet(string facetName, XdbModel xdbModel, EntityType entityType)
    {
      if (string.IsNullOrEmpty(facetName))
        throw new ArgumentNullException();
      if (xdbModel == null)
        throw new ArgumentNullException();
      XdbFacetDefinition facetDefinition;
      return xdbModel.TryGetFacetDefinition(entityType, facetName, out facetDefinition) ? Facet.CreateFromXdbType(facetDefinition.FacetType) : (Facet) null;
    }

    public Interaction ToInteraction(InteractionModel interactionModel)
    {
      ConvertResult<Interaction> convertResult = interactionModel != null ? this.InteractionConverter.Convert(interactionModel) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (Interaction) null : convertResult.ConvertedValue;
    }

    public InteractionModel ToInteractionModel(Interaction interaction)
    {
      ConvertResult<InteractionModel> convertResult = interaction != null ? this.InteractionModelConverter.Convert(interaction) : throw new ArgumentNullException();
      return !convertResult.WasConverted ? (InteractionModel) null : convertResult.ConvertedValue;
    }
  }
}
