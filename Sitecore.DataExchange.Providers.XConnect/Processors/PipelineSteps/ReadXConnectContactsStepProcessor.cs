// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps.ReadXConnectContactsStepProcessor
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.DataExchange.Providers.XConnect.Models;
using Sitecore.DataExchange.Providers.XConnect.Plugins;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.XConnect.Processors.PipelineSteps
{
  [RequiredEndpointPlugins(new Type[] {typeof (XConnectClientEndpointSettings)})]
  [RequiredPipelineStepPlugins(new Type[] {typeof (ReadEntitySettings), typeof (LoadInteractionsSettings)})]
  public class ReadXConnectContactsStepProcessor : BaseReadDataStepProcessor
  {
    public virtual BinaryExpression GetDeltaSettingsExpression(
      DateRangeSettings dateRangeSettings,
      ParameterExpression entityParameter)
    {
      DateTime? nullable1 = new DateTime?(dateRangeSettings.LowerBound);
      DateTime? nullable2 = new DateTime?(dateRangeSettings.UpperBound);
      ConstantExpression right1 = Expression.Constant((object) dateRangeSettings.LowerBound.ToUniversalTime(), typeof (DateTime?));
      ConstantExpression right2 = Expression.Constant((object) dateRangeSettings.UpperBound.ToUniversalTime(), typeof (DateTime?));
      MemberExpression left = Expression.Property((Expression) entityParameter, "LastModified");
      return Expression.AndAlso((Expression) Expression.LessThanOrEqual((Expression) left, (Expression) right2), (Expression) Expression.GreaterThanOrEqual((Expression) left, (Expression) right1));
    }

    public virtual IEnumerable<ContactModel> GetContactModelsAsIEnumerable(
      IAsyncEntityBatchEnumerator<Contact> cursor,
      int maxCount,
      IXConnectClientHelper clientHelper)
    {
      int i = 0;
      while (cursor.MoveNext<IReadOnlyCollection<Contact>>().Result)
      {
        foreach (Contact contact in (IEnumerable<Contact>) cursor.Current)
        {
          ContactModel contactModel = clientHelper.ToContactModel(contact);
          ++i;
          if (i >= maxCount || maxCount == 0)
            yield return contactModel;
        }
      }
    }

    protected override void ReadData(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
            var xConnectClientHelper = GetXConnectClientHelper(endpoint, pipelineStep, pipelineContext, logger);
            if (xConnectClientHelper == null)
            {
                pipelineContext.CriticalError = true;
                Log(logger.Error, pipelineContext, $"Cannot get xconnect client helper.");
                return;
            }

            var xConnectClientSettings = GetXConnectClientSettings(endpoint, pipelineStep, pipelineContext, logger);
            if (xConnectClientSettings == null)
            {
                pipelineContext.CriticalError = true;
                Log(logger.Error, pipelineContext, $"Cannot get xconnect client settings.");
                return;
            }

            var xConnectClient = xConnectClientHelper.ToXConnectClient(xConnectClientSettings);
            var expandOptions = ResolveExpandOptions(pipelineStep, pipelineContext, logger);
            var readContactSettings = GetReadEntitySettings(pipelineStep, pipelineContext, logger);

            var batchSize = GetActualBatchSize(readContactSettings);

            var contactFilterExpression = GetContactFilterExpression(pipelineStep, pipelineContext, logger);

            var dateRangeSettings = pipelineContext.GetPlugin<DateRangeSettings>();

            IAsyncEntityBatchEnumerator<Contact> cursor;
            if ((dateRangeSettings != null) && ((dateRangeSettings.LowerBound != DateTime.MinValue) || (dateRangeSettings.UpperBound != DateTime.MaxValue)))
            {
                Expression<Func<Contact, bool>> lambda;

                if (contactFilterExpression == null)
                {
                    ParameterExpression pe = Expression.Parameter(typeof(Contact), "c");
                    var deltaSettingsExpression = GetDeltaSettingsExpression(dateRangeSettings, pe);
                    lambda = Expression.Lambda<Func<Contact, bool>>(deltaSettingsExpression, pe);
                }
                else
                {
                    var deltaSettingsExpression = GetDeltaSettingsExpression(dateRangeSettings, contactFilterExpression.Parameters[0]);

                    // combine deltasettings expression with contactFilterExpression's body
                    var body = Expression.AndAlso(contactFilterExpression.Body, deltaSettingsExpression);
                    lambda = Expression.Lambda<Func<Contact, bool>>(body, contactFilterExpression.Parameters[0]);
                }

                cursor = xConnectClient.Contacts
                    .Where(lambda)
                    .WithExpandOptions(expandOptions).GetBatchEnumerator(batchSize).Result;
            }
            else
            {
                cursor = contactFilterExpression == null
                    ? xConnectClient.Contacts.WithExpandOptions(expandOptions).GetBatchEnumerator(batchSize).Result
                    : xConnectClient.Contacts
                        .Where(contactFilterExpression)
                        .WithExpandOptions(expandOptions).GetBatchEnumerator(batchSize).Result;
            }

            IEnumerable<ContactModel> entities = GetContactModelsAsIEnumerable(cursor, readContactSettings.MaxCount, xConnectClientHelper);

            var iterableDataSettings = new IterableDataSettings(entities);

            var msg = (cursor.TotalCount > 0) ? $"Entities were read from xConnect. (total count: {cursor.TotalCount})" : "No entities were read from xConnect.";
            Log(logger.Info, pipelineContext, msg, $"endpoint: {endpoint.Name}", $"entity: contact");
            pipelineContext.AddPlugin(iterableDataSettings);
        }

    protected virtual Expression<Func<Contact, bool>> GetContactFilterExpression(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return this.GetReadEntitySettings(pipelineStep, pipelineContext, logger)?.EntityExpressionBuilder?.Build<Contact>(new ExpressionContext());
    }

    protected virtual int GetActualBatchSize(ReadEntitySettings settings)
    {
      int num = settings.BatchSize < 1 ? settings.DefaultBatchSize : settings.BatchSize;
      int maxCount = settings.MaxCount;
      return maxCount == 0 || maxCount >= num ? num : maxCount;
    }

    protected virtual LoadInteractionsSettings GetLoadInteractionsSettings(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<LoadInteractionsSettings>();
    }

    protected virtual ReadEntitySettings GetReadEntitySettings(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return pipelineStep.GetPlugin<ReadEntitySettings>();
    }

    protected virtual IXConnectClientHelper GetXConnectClientHelper(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return endpoint?.GetPlugin<XConnectClientEndpointSettings>()?.ClientHelper;
    }

    protected virtual XConnectClientSettings GetXConnectClientSettings(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return endpoint?.GetPlugin<XConnectClientEndpointSettings>()?.ClientSettings;
    }

    protected virtual ContactExpandOptions ResolveExpandOptions(
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      ContactExpandOptions contactExpandOptions = new ContactExpandOptions(this.GetReadEntitySettings(pipelineStep, pipelineContext, logger).FacetNames.ToArray());
      LoadInteractionsSettings interactionsSettings = this.GetLoadInteractionsSettings(pipelineStep, pipelineContext, logger);
      if (interactionsSettings.LoadInteractions)
        contactExpandOptions.Interactions = new RelatedInteractionsExpandOptions(interactionsSettings.FacetNames.ToArray<string>())
        {
          Limit = new int?(interactionsSettings.MaxCount)
        };
      return contactExpandOptions;
    }

    private async Task<List<TEntity>> GetEntitiesFromCursor<TEntity>(
      IAsyncEntityBatchEnumerator<TEntity> cursor,
      int maxCount)
      where TEntity : Entity
    {
      List<TEntity> items = new List<TEntity>();
      int i = 0;
label_9:
      if (!await ((IAsyncEnumerator<IReadOnlyCollection<TEntity>>) cursor).MoveNext<IReadOnlyCollection<TEntity>>())
        return items;
      using (IEnumerator<TEntity> enumerator = ((IAsyncEnumerator<IReadOnlyCollection<TEntity>>) cursor).Current.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          items.Add(enumerator.Current);
          ++i;
          if (i >= maxCount && maxCount > 0)
            return items;
        }
        goto label_9;
      }
    }

    private async Task<List<EntityModel>> GetContactModelsFromCursor(
      IAsyncEntityBatchEnumerator<Contact> cursor,
      int maxCount,
      IXConnectClientHelper clientHelper)
    {
      List<EntityModel> items = new List<EntityModel>();
      int i = 0;
label_9:
      if (!await cursor.MoveNext<IReadOnlyCollection<Contact>>())
        return items;
      using (IEnumerator<Contact> enumerator = cursor.Current.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          items.Add((EntityModel) clientHelper.ToContactModel(enumerator.Current));
          ++i;
          if (i >= maxCount && maxCount > 0)
            return items;
        }
        goto label_9;
      }
    }
  }
}
