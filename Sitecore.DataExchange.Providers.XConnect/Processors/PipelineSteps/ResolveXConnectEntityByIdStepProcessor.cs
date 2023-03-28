// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ResolveXConnectEntityByIdStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using System;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  public class ResolveXConnectEntityByIdStepProcessor : BaseResolveEntityFromXConnectStepProcessor
  {
    public override object CreateNewObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (string.IsNullOrWhiteSpace(identifierValue))
        throw new ArgumentException("The value cannot be null, empty or whitespace.", nameof (identifierValue));
      Guid result = Guid.Empty;
      if (!Guid.TryParse(identifierValue, out result))
        throw new ArgumentOutOfRangeException(nameof (identifierValue), "The value must be a guid.");
      Endpoint endpoint = this.GetEndpoint(pipelineStep, pipelineContext, logger);
      if (endpoint == null)
        throw new ArgumentNullException("endpoint");
      if (pipelineStep == null)
        throw new ArgumentNullException(nameof (pipelineStep));
      if (pipelineContext == null)
        throw new ArgumentNullException(nameof (pipelineContext));
      ResolveEntitySettings plugin = pipelineStep.GetPlugin<ResolveEntitySettings>();
      if (plugin == null)
        return (object) null;
      IXConnectClientHelper xconnectClientHelper = this.GetXConnectClientHelper(pipelineStep, pipelineContext, logger);
      if (xconnectClientHelper == null)
        return (object) null;
      IXdbContext xconnectClient = this.GetXConnectClient(pipelineStep, pipelineContext, logger);
      if (xconnectClient == null)
        return (object) null;
      this.SetRepositoryStatusSettings(RepositoryObjectStatus.DoesNotExist, pipelineContext);
      EntityModel newObject = this.GetCreateEntityModelDelegate(plugin, logger)(result, xconnectClient, xconnectClientHelper, logger);
      if (newObject != null)
        return (object) newObject;
      this.Log(new Action<string>(logger.Error), pipelineContext, "Entity could not be created.", new string[3]
      {
        "endpoint: " + endpoint.Name,
        string.Format("entity type: {0}", (object) plugin.EntityType),
        "identifier value: " + identifierValue
      });
      return (object) null;
    }

    protected override object FindExistingObject(
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Guid result = Guid.Empty;
      if (!Guid.TryParse(identifierValue, out result))
        throw new ArgumentOutOfRangeException(nameof (identifierValue), "The value must be a guid.");
      ResolveEntitySettings plugin = pipelineStep.GetPlugin<ResolveEntitySettings>();
      ExpandOptions expandOptions = this.GetExpandOptions(plugin, logger);
      object existingObject = this.GetObjectFromXConnectResolveDelegate(plugin, logger)(result, expandOptions, client, clientHelper, logger);
      if (existingObject == null)
        return (object) null;
      this.SetRepositoryStatusSettings(RepositoryObjectStatus.Exists, pipelineContext);
      return existingObject;
    }

    protected virtual ExpandOptions GetExpandOptions(
      ResolveEntitySettings settings,
      ILogger logger)
    {
      return new ExpandOptions(settings.FacetNames.ToArray());
    }

    protected virtual Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object> GetObjectFromXConnectResolveDelegate(
      ResolveEntitySettings settings,
      ILogger logger)
    {
      if (settings.DoNotConvertToEntityModel)
      {
        switch (settings.EntityType)
        {
          case EntityType.Contact:
            return new Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object>(this.GetContact);
          case EntityType.Interaction:
            return new Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object>(this.GetInteraction);
          case EntityType.DeviceProfile:
            return new Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object>(this.GetDeviceProfileAsEntityModel);
        }
      }
      switch (settings.EntityType)
      {
        case EntityType.Contact:
          return new Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object>(this.GetContactAsEntityModel);
        case EntityType.Interaction:
          return new Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object>(this.GetInteractionAsEntityModel);
        case EntityType.DeviceProfile:
          return new Func<Guid, ExpandOptions, IXdbContext, IXConnectClientHelper, ILogger, object>(this.GetDeviceProfileAsEntityModel);
        default:
          throw new NotSupportedException(settings.EntityType.ToString());
      }
    }

    protected virtual Contact GetContact(
      Guid id,
      ExpandOptions expandOptions,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      ContactExpandOptions expandOptions1 = new ContactExpandOptions(expandOptions.FacetKeys.Count > 0 ? expandOptions.FacetKeys.ToArray<string>() : new string[0]);
      return client.Get<Contact>((IEntityReference<Contact>) new ContactReference(id), (ExecutionOptions<Contact>) new ContactExecutionOptions(expandOptions1));
    }

    protected virtual EntityModel GetContactAsEntityModel(
      Guid id,
      ExpandOptions expandOptions,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      Contact contact = this.GetContact(id, expandOptions, client, clientHelper, logger);
      return contact == null ? (EntityModel) null : (EntityModel) clientHelper.ToContactModel(contact);
    }

    protected virtual DeviceProfile GetDeviceProfile(
      Guid id,
      ExpandOptions expandOptions,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      DeviceProfileExpandOptions expandOptions1 = new DeviceProfileExpandOptions(expandOptions.FacetKeys.Count > 0 ? expandOptions.FacetKeys.ToArray<string>() : new string[0]);
      return client.Get<DeviceProfile>((IEntityReference<DeviceProfile>) new DeviceProfileReference(id), (ExecutionOptions<DeviceProfile>) new DeviceProfileExecutionOptions(expandOptions1));
    }

    protected virtual EntityModel GetDeviceProfileAsEntityModel(
      Guid id,
      ExpandOptions expandOptions,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      DeviceProfile deviceProfile = this.GetDeviceProfile(id, expandOptions, client, clientHelper, logger);
      return deviceProfile == null ? (EntityModel) null : (EntityModel) clientHelper.ToDeviceProfileModel(deviceProfile);
    }

    protected virtual Interaction GetInteraction(
      Guid id,
      ExpandOptions expandOptions,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      throw new NotImplementedException();
    }

    protected virtual EntityModel GetInteractionAsEntityModel(
      Guid id,
      ExpandOptions expandOptions,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      throw new NotImplementedException();
    }

    protected virtual Func<Guid, IXdbContext, IXConnectClientHelper, ILogger, EntityModel> GetCreateEntityModelDelegate(
      ResolveEntitySettings settings,
      ILogger logger)
    {
      switch (settings.EntityType)
      {
        case EntityType.Contact:
          return new Func<Guid, IXdbContext, IXConnectClientHelper, ILogger, EntityModel>(this.CreateContact);
        case EntityType.Interaction:
          return new Func<Guid, IXdbContext, IXConnectClientHelper, ILogger, EntityModel>(this.CreateInteraction);
        case EntityType.DeviceProfile:
          return new Func<Guid, IXdbContext, IXConnectClientHelper, ILogger, EntityModel>(this.CreateDeviceProfile);
        default:
          throw new NotSupportedException(settings.EntityType.ToString());
      }
    }

    protected virtual EntityModel CreateContact(
      Guid id,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      return (EntityModel) new ContactModel();
    }

    protected virtual EntityModel CreateDeviceProfile(
      Guid id,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      DeviceProfileModel deviceProfile = new DeviceProfileModel();
      deviceProfile.Id = id;
      return (EntityModel) deviceProfile;
    }

    protected virtual EntityModel CreateInteraction(
      Guid id,
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      ILogger logger)
    {
      return (EntityModel) new InteractionModel();
    }
  }
}
