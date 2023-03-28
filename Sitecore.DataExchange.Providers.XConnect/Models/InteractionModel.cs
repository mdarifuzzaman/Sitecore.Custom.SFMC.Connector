// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Models.InteractionModel
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Models
{
  public class InteractionModel : ReadOnlyEntityModel<Interaction>
  {
    public InteractionModel()
    {
    }

    public InteractionModel(
      IEntityReference<Contact> contactReference,
      InteractionInitiator initiator,
      Guid сhannelId,
      string userAgent)
    {
      this.ContactReference = contactReference;
      this.Initiator = initiator;
      this.ChannelId = сhannelId;
      this.UserAgent = userAgent;
    }

    [JsonIgnore]
    public IEntityReference<Contact> ContactReference { get; set; }

    public InteractionInitiator Initiator { get; set; }

    public Guid CampaignId { get; set; }

    public Guid ChannelId { get; set; }

    [JsonIgnore]
    public IEntityReference<Sitecore.XConnect.DeviceProfile> DeviceProfile { get; set; }

    public Guid VenueId { get; set; }

    public int EngagementValue { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string UserAgent { get; set; }

    public List<Event> Events { get; set; } = new List<Event>();

    protected override Interaction GetEntity(IXConnectClientHelper helper) => helper.ToInteraction(this);

    protected override IEntityReference GetEntityReference(Interaction entity) => (IEntityReference) entity;

    protected override IEnumerable<IXdbOperation> GetOperationsForEntity(
      Interaction entity,
      IXdbContext client,
      ILogger logger)
    {
      if (entity == null)
      {
        this.LogError("Null interactions cannot be added to the xConnect client.", logger);
        return Enumerable.Empty<IXdbOperation>();
      }
      if (entity == null || entity.Events == null || entity.Events.Count == 0)
      {
        this.LogError("The interaction must have at least one event in order to be added to the xConnect client.", logger);
        return (IEnumerable<IXdbOperation>) null;
      }
      List<IXdbOperation> operationsForEntity = new List<IXdbOperation>();
      operationsForEntity.Add((IXdbOperation) client.AddInteraction(entity));
      if (this.Facets != null && this.Facets.Count > 0)
      {
        foreach (KeyValuePair<string, Facet> facet in (IEnumerable<KeyValuePair<string, Facet>>) this.Facets)
          operationsForEntity.Add((IXdbOperation) client.SetFacet<Facet>((IEntityReference) entity, facet.Key, facet.Value));
      }
      return (IEnumerable<IXdbOperation>) operationsForEntity;
    }

    protected virtual void LogError(string message, ILogger logger) => logger.Error(message + " (entity model internal description: " + this.InternalDescription + ")");
  }
}
