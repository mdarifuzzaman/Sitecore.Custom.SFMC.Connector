// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ResolveInteractionFromXConnectContactStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (ResolveObjectSettings), typeof (ResolveIdentifierSettings), typeof (EntityExpressionBuilderSettings), typeof (XConnectContactIdentifierSettings), typeof (EntityFacetSettings)})]
  public class ResolveInteractionFromXConnectContactStepProcessor : 
    BaseResolveEntityFromXConnectContactStepProcessor<Interaction, InteractionModel>
  {
    public override object FindExistingObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (string.IsNullOrWhiteSpace(identifierValue))
        throw new ArgumentException("The value cannot be null, empty or whitespace.", nameof (identifierValue));
      if (pipelineStep == null)
        throw new ArgumentNullException(nameof (pipelineStep));
      if (pipelineContext == null)
        throw new ArgumentNullException(nameof (pipelineContext));
      if (pipelineStep.GetPlugin<ResolveObjectSettings>() == null)
        return (object) null;
      ContactModel contactModel = this.GetContactModel(pipelineStep, pipelineContext, logger);
      if (contactModel == null)
        return (object) null;
      if (contactModel.Entity == null)
        return (object) null;
      if (!(contactModel.Entity is Contact entity))
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The entity assigned to the contact model is not a contact.", new string[2]
        {
          "identifier id: " + identifierValue,
          "entity type: " + contactModel.Entity.GetType().FullName
        });
        return (object) null;
      }
      Expression<Func<Interaction, bool>> filterExpression = this.CreateFilterExpression(identifierValue, pipelineStep);
      if (filterExpression == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot create filter expression to resolve interaction.", new string[1]
        {
          "identifier id: " + identifierValue
        });
        return (object) null;
      }
      if (this.ResolveEntityFromContact(filterExpression, entity, pipelineStep, pipelineContext, logger) == null)
        return (object) null;
      InteractionModel entityModel = this.CreateEntityModel(entity, pipelineStep, pipelineContext, logger);
      if (entityModel != null)
        return (object) entityModel;
      this.Log(new Action<string>(logger.Error), pipelineContext, "The interaction model was not created.", new string[1]
      {
        "identifier id: " + identifierValue
      });
      return (object) null;
    }

    public override object CreateNewObject(
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ContactModel contactModel = this.GetContactModel(pipelineStep, pipelineContext, logger);
      if (contactModel == null)
        return (object) null;
      Contact contact = this.GetContact(contactModel, pipelineStep, pipelineContext, logger);
      if (contact == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The entity assigned to the contact model is not a contact.", new string[2]
        {
          "identifier id: " + identifierValue,
          "entity type: " + contactModel.Entity.GetType().FullName
        });
        return (object) null;
      }
      InteractionModel entityModel = this.CreateEntityModel(contact, pipelineStep, pipelineContext, logger);
      if (entityModel != null)
        return (object) entityModel;
      this.Log(new Action<string>(logger.Error), pipelineContext, "The interaction model was not created.", new string[1]
      {
        "identifier id: " + identifierValue
      });
      return (object) null;
    }

    protected override InteractionModel CreateEntityModel(
      Contact contact,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return new InteractionModel()
      {
        ContactReference = (IEntityReference<Contact>) contact,
        Initiator = InteractionInitiator.Contact
      };
    }

    protected virtual ICollection<string> GetFacetNames(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<EntityFacetSettings>().FacetNames;
    }

    protected override Interaction ResolveEntityFromContact(
      Expression<Func<Interaction, bool>> expression,
      Contact contact,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return contact.Interactions == null || !contact.Interactions.Any<Interaction>() ? (Interaction) null : contact.Interactions.AsQueryable<Interaction>().FirstOrDefault<Interaction>(expression);
    }

    protected virtual Contact GetContact(
      ContactModel contactModel,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return contactModel.Entity == null ? (Contact) null : contactModel.Entity as Contact;
    }

    protected override InteractionModel ConvertEntityToEntityModel(
      Interaction entity,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      throw new NotImplementedException();
    }
  }
}
