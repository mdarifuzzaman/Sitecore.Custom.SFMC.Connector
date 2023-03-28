// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Models.ContactModel
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Newtonsoft.Json;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Models
{
  public class ContactModel : EntityModel
  {
    [JsonIgnore]
    public IReadOnlyCollection<Interaction> Interactions { get; set; }

    public List<ContactIdentifier> ContactIdentifiers { get; set; } = new List<ContactIdentifier>();

    public override Entity AddToClient(
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationsAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Contact contact = helper.ToContact(this);
      if (this.Id == Guid.Empty)
      {
        AddContactOperation contactOperation = client.AddContact(contact);
        onOperationsAddedToClient((IEnumerable<IXdbOperation>) new AddContactOperation[1]
        {
          contactOperation
        }, client, minBatchSize, pipelineStep, pipelineContext, logger);
      }
      this.AddFacets(this.GetEntityReference(contact), client, helper, onOperationsAddedToClient, minBatchSize, pipelineStep, pipelineContext, logger);
      this.AddIdentifiers(contact, client, helper, onOperationsAddedToClient, minBatchSize, pipelineStep, pipelineContext, logger);
      return (Entity) contact;
    }

    protected virtual IEntityReference GetEntityReference(Contact contact) => this.Id == Guid.Empty ? (IEntityReference) contact : (IEntityReference) new ContactReference(this.Id);

    protected virtual void AddIdentifiers(
      Contact entity,
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationsAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (this.ContactIdentifiers == null || this.ContactIdentifiers.Count == 0)
        return;
      List<IXdbOperation> xdbOperationList = new List<IXdbOperation>();
      foreach (ContactIdentifier contactIdentifier1 in this.ContactIdentifiers)
      {
        ContactIdentifier contactIdentifier = contactIdentifier1;
        if (entity.Identifiers.All<ContactIdentifier>((Func<ContactIdentifier, bool>) (i => i.Source != contactIdentifier.Source)))
        {
          AddContactIdentifierOperation identifierOperation = client.AddContactIdentifier((IEntityReference<Contact>) entity, contactIdentifier);
          onOperationsAddedToClient((IEnumerable<IXdbOperation>) new AddContactIdentifierOperation[1]
          {
            identifierOperation
          }, client, minBatchSize, pipelineStep, pipelineContext, logger);
        }
      }
    }
  }
}
