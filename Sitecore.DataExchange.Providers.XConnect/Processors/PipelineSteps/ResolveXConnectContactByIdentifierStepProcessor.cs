// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ResolveXConnectContactByIdentifierStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.Retryers;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredPipelineStepPlugins(new Type[] {typeof (XConnectContactIdentifierSettings), typeof (LoadInteractionsSettings), typeof (RetrySettings)})]
  public class ResolveXConnectContactByIdentifierStepProcessor : 
    BaseResolveEntityFromXConnectStepProcessor
  {
    public override object CreateNewObject(
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
      if (pipelineStep.GetResolveObjectSettings() == null)
        return (object) null;
      string identifierSource = this.GetIdentifierSource(pipelineStep);
      if (string.IsNullOrWhiteSpace(identifierSource))
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The specified contact identifier source value is either null or empty.", Array.Empty<string>());
        return (object) null;
      }
      this.GetContactExpandOptions(pipelineStep, pipelineContext, logger);
      this.SetRepositoryStatusSettings(RepositoryObjectStatus.DoesNotExist, pipelineContext);
      ContactIdentifierType contactIdentifierType = this.GetContactIdentifierType(pipelineStep, pipelineContext, logger);
      ContactIdentifier contactIdentifier = new ContactIdentifier(identifierSource, identifierValue, contactIdentifierType);
      return (object) new ContactModel()
      {
        ContactIdentifiers = new List<ContactIdentifier>()
        {
          contactIdentifier
        }
      };
    }

    protected override object FindExistingObject(
      IXdbContext client,
      IXConnectClientHelper clientHelper,
      string identifierValue,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      string identifierSource = this.GetIdentifierSource(pipelineStep);
      if (string.IsNullOrWhiteSpace(identifierSource))
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The specified contact identifier source value is either null or empty.", Array.Empty<string>());
        return (object) null;
      }
      IdentifiedContactReference reference = new IdentifiedContactReference(identifierSource, identifierValue);
      ContactExpandOptions contactExpandOptions = this.GetContactExpandOptions(pipelineStep, pipelineContext, logger);
      Contact contact = (Contact) null;
      IRetryerOnException retryer = this.GetRetryer(pipelineStep, pipelineContext, logger);
      if (retryer == null)
        contact = client.Get<Contact>((IEntityReference<Contact>) reference, (ExecutionOptions<Contact>) new ContactExecutionOptions(contactExpandOptions));
      else
        retryer.Retry((Action) (() => contact = client.Get<Contact>((IEntityReference<Contact>) reference, (ExecutionOptions<Contact>) new ContactExecutionOptions(contactExpandOptions))));
      if (contact == null)
        return (object) null;
      this.SetRepositoryStatusSettings(RepositoryObjectStatus.Exists, pipelineContext);
      return pipelineStep.GetPlugin<ResolveEntitySettings>().DoNotConvertToEntityModel ? (object) contact : (object) clientHelper.ToContactModel(contact);
    }

    protected virtual IRetryerOnException GetRetryer(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      IRetryerOnException retryer = pipelineStep != null ? pipelineStep.GetRetrySettings()?.RetryerOnException : (IRetryerOnException) null;
      if (retryer == null)
        return (IRetryerOnException) null;
      retryer.Logger = logger;
      return retryer;
    }

    protected virtual ContactExpandOptions GetContactExpandOptions(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return new ContactExpandOptions(this.GetResolveEntitySettings(pipelineStep, pipelineContext).FacetNames.ToArray())
      {
        Interactions = this.GetRelatedInteractionsExpandOptions(pipelineStep, pipelineContext, logger)
      };
    }

    protected virtual RelatedInteractionsExpandOptions GetRelatedInteractionsExpandOptions(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      LoadInteractionsSettings plugin = pipelineStep.GetPlugin<LoadInteractionsSettings>();
      return plugin == null || !plugin.LoadInteractions ? (RelatedInteractionsExpandOptions) null : new RelatedInteractionsExpandOptions(plugin.FacetNames.ToArray<string>());
    }

    protected virtual ContactIdentifierType GetContactIdentifierType(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      XConnectContactIdentifierSettings plugin = pipelineStep.GetPlugin<XConnectContactIdentifierSettings>();
      if (plugin != null && plugin.KnownContactValueReaders.Any<IValueReader>())
      {
        object fromPipelineContext = this.GetObjectFromPipelineContext(plugin.ContactIdentificationLevelObjectLocation, pipelineContext, logger);
        if (fromPipelineContext != null)
        {
          IValueAccessor levelValueAccessor = plugin.ContactIdentificationLevelValueAccessor;
          if (levelValueAccessor != null)
          {
            IValueReader valueReader = levelValueAccessor.ValueReader;
            if (valueReader != null)
            {
              DataAccessContext context = new DataAccessContext();
              ReadResult readResult1 = valueReader.Read(fromPipelineContext, context);
              if (readResult1 != null && readResult1.WasValueRead)
              {
                object readValue = readResult1.ReadValue;
                foreach (IValueReader contactValueReader in plugin.KnownContactValueReaders)
                {
                  ReadResult readResult2 = contactValueReader.Read(fromPipelineContext, context);
                  if (readResult2 != null && readResult2.WasValueRead && object.Equals(readValue, readResult2.ReadValue))
                    return ContactIdentifierType.Known;
                }
              }
            }
          }
        }
      }
      return ContactIdentifierType.Anonymous;
    }

    private string GetIdentifierSource(PipelineStep pipelineStep)
    {
      XConnectContactIdentifierSettings plugin = pipelineStep.GetPlugin<XConnectContactIdentifierSettings>();
      if (plugin != null)
      {
        string identifierSource = plugin.IdentifierSource;
        if (!string.IsNullOrWhiteSpace(identifierSource))
          return identifierSource;
      }
      return (string) null;
    }
  }
}
