// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ReadInteractionsFromContactStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Framework.Conditions;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (ReadEntitySettings), typeof (XConnectContactIdentifierSettings)})]
  public class ReadInteractionsFromContactStepProcessor : BasePipelineStepProcessor
  {
    protected readonly IXConnectClientHelper ClientHelper;

    public ReadInteractionsFromContactStepProcessor()
      : this((IXConnectClientHelper) new DefaultXConnectClientHelper())
    {
    }

    public ReadInteractionsFromContactStepProcessor(IXConnectClientHelper clientHelper) => this.ClientHelper = clientHelper;

    protected override void ProcessPipelineStep(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Condition.Requires<PipelineStep>(pipelineStep).IsNotNull<PipelineStep>();
      Condition.Requires<PipelineContext>(pipelineContext).IsNotNull<PipelineContext>();
      Condition.Requires<ILogger>(logger).IsNotNull<ILogger>();
      this.ReadData(pipelineStep, pipelineContext, logger);
    }

    protected virtual void ReadData(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Expression<Func<Interaction, bool>> filterExpression = this.GetOrCreateFilterExpression(pipelineStep, pipelineContext, logger);
      Contact contact = this.GetContact(pipelineStep, pipelineContext, logger);
      if (contact == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Contct is null.", Array.Empty<string>());
      }
      else
      {
        IEnumerable<Interaction> interactions = this.ReadEntityFromContact(filterExpression, contact, pipelineStep, pipelineContext, logger);
        IEnumerable data = !this.ShouldConvertToEntityModel(pipelineStep, pipelineContext, logger) ? (IEnumerable) interactions : (IEnumerable) this.ConvertIntearctionsToInteractionModels(interactions, pipelineStep, pipelineContext, logger);
        pipelineContext.AddPlugin<IterableDataSettings>(new IterableDataSettings(data));
      }
    }

    protected virtual Contact GetContact(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      Guid contactLocation = this.GetXConnectContactIdentifierSettings(pipelineStep, pipelineContext, logger).ContactLocation;
      if (contactLocation == Guid.Empty)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "Cannot resolve contact location.", Array.Empty<string>());
        return (Contact) null;
      }
      if (this.GetObjectFromPipelineContext(contactLocation, pipelineContext, logger) is ContactModel fromPipelineContext)
        return fromPipelineContext.Entity as Contact;
      this.Log(new Action<string>(logger.Error), pipelineContext, string.Format("Cannot get contact by conact location. (contact location id: {0})", (object) contactLocation), Array.Empty<string>());
      return (Contact) null;
    }

    protected virtual XConnectContactIdentifierSettings GetXConnectContactIdentifierSettings(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<XConnectContactIdentifierSettings>();
    }

    protected virtual Expression<Func<Interaction, bool>> GetOrCreateFilterExpression(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return this.GetReadEntitySettings(pipelineStep, pipelineContext, logger)?.EntityExpressionBuilder?.Build<Interaction>(new ExpressionContext());
    }

    protected virtual ReadEntitySettings GetReadEntitySettings(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<ReadEntitySettings>();
    }

    protected virtual IEnumerable<Interaction> ReadEntityFromContact(
      Expression<Func<Interaction, bool>> expression,
      Contact contact,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      if (contact.Interactions == null || !contact.Interactions.Any<Interaction>())
        return Enumerable.Empty<Interaction>();
      return expression != null ? (IEnumerable<Interaction>) contact.Interactions.AsQueryable<Interaction>().Where<Interaction>(expression) : (IEnumerable<Interaction>) contact.Interactions.AsQueryable<Interaction>();
    }

    protected virtual IEnumerable<InteractionModel> ConvertIntearctionsToInteractionModels(
      IEnumerable<Interaction> interactions,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      foreach (Interaction interaction in interactions)
        yield return this.ClientHelper.ToInteractionModel(interaction);
    }

    protected virtual InteractionModel CreateEntityModel(
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

    protected virtual bool ShouldConvertToEntityModel(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return false;
    }
  }
}
