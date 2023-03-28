// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification.DummyXConnectClient
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification
{
  public class DummyXConnectClient : IXdbContext, IReadOnlyXdbContext, IDisposable
  {
    private List<IXdbOperation> _operations = new List<IXdbOperation>();
    private IDictionary<Type, List<Entity>> _createdObjects = (IDictionary<Type, List<Entity>>) new Dictionary<Type, List<Entity>>();

    public DummyXConnectClient(IXdbContext client) => this.Client = client;

    public event EventHandler<XdbOperationBatchEventArgs> BatchExecuted;

    public event AsyncEventHandler<XdbOperationBatchEventArgs> BatchExecutedAsync;

    public event EventHandler<XdbOperationBatchEventArgs> BatchExecuting;

    public event AsyncEventHandler<XdbOperationBatchEventArgs> BatchExecutingAsync;

    public event EventHandler<XdbOperationBatchFailedEventArgs> BatchExecutionFailed;

    public event AsyncEventHandler<XdbOperationBatchFailedEventArgs> BatchExecutionFailedAsync;

    public event EventHandler<XdbOperationEventArgs> OperationAdded;

    public event EventHandler<XdbOperationEventArgs> OperationCompleted;

    public event EventHandler<XdbOperationEventArgs> OperationExecuting;

    public XdbContextConfiguration Configuration => this.Client.Configuration;

    public byte[] SyncToken { get; set; }

    public Guid LastBatchId { get; set; }

    public ReadOnlyCollection<IXdbOperation> LastBatch { get; set; }

    public Collection<XdbOperationInvoker> OperationInvokers { get; set; }

    public IReadOnlyCollection<IXdbOperation> DirectOperations => (IReadOnlyCollection<IXdbOperation>) this._operations.ToArray();

    public XdbModel Model => this.Client.Model;

    public IAsyncQueryable<Contact> Contacts { get; set; }

    public IAsyncQueryable<Interaction> Interactions { get; set; }

    protected IXdbContext Client { get; private set; }

    public IReadOnlyXdbContext AsReadOnly() => (IReadOnlyXdbContext) this;

    public Task<IAsyncEntityBatchEnumerator<Contact>> CreateContactEnumerator(
      ExpandOptions expandOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncEntityBatchEnumerator<Contact>> CreateContactEnumerator(
      ExpandOptions expandOptions,
      SamplingOptions samplingOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncEntityBatchEnumerator<Contact>> CreateContactEnumerator(
      byte[] bookmark,
      ExpandOptions expandOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      DateTime cutoffDate,
      DateTime minStartDateTime,
      DateTime maxStartDateTime,
      ExpandOptions expandOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      DateTime cutoffDate,
      DateTime minStartDateTime,
      DateTime maxStartDateTime,
      ExpandOptions expandOptions,
      SamplingOptions samplingOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      DateTime cutoffDate,
      ExpandOptions expandOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      DateTime cutoffDate,
      ExpandOptions expandOptions,
      SamplingOptions samplingOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      byte[] bookmark,
      ExpandOptions expandOptions,
      int batchSize)
    {
      throw new NotImplementedException();
    }

    public void Dispose() => this.Client.Dispose();

    public Task ExecuteAsync(IEnumerable<IXdbOperation> operations) => throw new NotImplementedException();

    public Task ExecuteAsync(
      IEnumerable<IXdbOperation> operations,
      CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<TResult> ExecuteSingleOperation<TOperation, TResult>(
      TOperation operation,
      Func<TOperation, TResult> resultSelector)
      where TOperation : IXdbOperation
    {
      throw new NotImplementedException();
    }

    public Task<ReadOnlyCollection<IEntityLookupResult<TEntity>>> GetAsync<TEntity>(
      IReadOnlyCollection<IEntityReference<TEntity>> references,
      ExpandOptions expandOptions)
      where TEntity : Entity
    {
      List<IEntityLookupResult<TEntity>> list = new List<IEntityLookupResult<TEntity>>();
      IEnumerable<TEntity> entities = this.GetEntities<TEntity>();
      foreach (IEntityReference<TEntity> reference1 in (IEnumerable<IEntityReference<TEntity>>) references)
      {
        IEntityReference<TEntity> reference = reference1;
        TEntity entity = entities.FirstOrDefault<TEntity>((Func<TEntity, bool>) (v =>
        {
          Guid? id1 = v.Id;
          Guid? id2 = reference.Id;
          if (id1.HasValue != id2.HasValue)
            return false;
          return !id1.HasValue || id1.GetValueOrDefault() == id2.GetValueOrDefault();
        }));
        if ((object) entity == null)
          list.Add((IEntityLookupResult<TEntity>) new DummyEntityLookupResult<TEntity>(reference));
        else
          list.Add((IEntityLookupResult<TEntity>) new DummyEntityLookupResult<TEntity>(entity));
      }
      return Task.FromResult<ReadOnlyCollection<IEntityLookupResult<TEntity>>>(new ReadOnlyCollection<IEntityLookupResult<TEntity>>((IList<IEntityLookupResult<TEntity>>) list));
    }

    public async Task<ReadOnlyCollection<IEntityLookupResult<TEntity>>> GetAsync<TEntity>(
      IReadOnlyCollection<IEntityReference<TEntity>> references,
      ExpandOptions expandOptions,
      TimeSpan timeout)
      where TEntity : Entity
    {
      return await this.GetAsync<TEntity>(references, expandOptions);
    }

    public async Task<ReadOnlyCollection<IEntityLookupResult<TEntity>>> GetAsync<TEntity>(
      IReadOnlyCollection<IEntityReference<TEntity>> references,
      ExpandOptions expandOptions,
      CancellationToken cancellationToken)
      where TEntity : Entity
    {
      return await this.GetAsync<TEntity>(references, expandOptions);
    }

    public async Task<TEntity> GetAsync<TEntity>(
      IEntityReference<TEntity> reference,
      ExpandOptions expandOptions)
      where TEntity : Entity
    {
      IEntityLookupResult<TEntity> entityLookupResult = (await this.GetAsync<TEntity>((IReadOnlyCollection<IEntityReference<TEntity>>) new IEntityReference<TEntity>[1]
      {
        reference
      }, expandOptions)).FirstOrDefault<IEntityLookupResult<TEntity>>();
      return entityLookupResult != null ? entityLookupResult.Entity : default (TEntity);
    }

    public Task<TEntity> GetAsync<TEntity>(
      IEntityReference<TEntity> reference,
      ExpandOptions expandOptions,
      TimeSpan timeout)
      where TEntity : Entity
    {
      return this.GetAsync<TEntity>(reference, expandOptions);
    }

    public Task<TEntity> GetAsync<TEntity>(
      IEntityReference<TEntity> reference,
      ExpandOptions expandOptions,
      CancellationToken cancellationToken)
      where TEntity : Entity
    {
      return this.GetAsync<TEntity>(reference, expandOptions);
    }

    public Task<ReadOnlyCollection<IEntityLookupResult<TEntity>>> GetAsync<TEntity>(
      IReadOnlyCollection<IEntityReference<TEntity>> references,
      ExecutionOptions<TEntity> executionOptions,
      TimeSpan timeout)
      where TEntity : Entity
    {
      return this.GetAsync<TEntity>(references, executionOptions.ExpandOptions, timeout);
    }

    public Task<ReadOnlyCollection<IEntityLookupResult<TEntity>>> GetAsync<TEntity>(
      IReadOnlyCollection<IEntityReference<TEntity>> references,
      ExecutionOptions<TEntity> executionOptions,
      CancellationToken cancellationToken)
      where TEntity : Entity
    {
      return this.GetAsync<TEntity>(references, executionOptions.ExpandOptions, cancellationToken);
    }

    public Task<TEntity> GetAsync<TEntity>(
      IEntityReference<TEntity> reference,
      ExecutionOptions<TEntity> executionOptions,
      TimeSpan timeout)
      where TEntity : Entity
    {
      return this.GetAsync<TEntity>(reference, executionOptions, timeout);
    }

    public Task<TEntity> GetAsync<TEntity>(
      IEntityReference<TEntity> reference,
      ExecutionOptions<TEntity> executionOptions,
      CancellationToken cancellationToken)
      where TEntity : Entity
    {
      return this.GetAsync<TEntity>(reference, executionOptions, cancellationToken);
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      InteractionEnumeratorOptions enumeratorOptions)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncInteractionBatchEnumerator> CreateInteractionEnumerator(
      BookmarkInteractionEnumeratorOptions enumeratorOptions)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncEntityBatchEnumerator<Contact>> CreateContactEnumerator(
      ContactEnumeratorOptions enumeratorOptions)
    {
      throw new NotImplementedException();
    }

    public Task<IAsyncEntityBatchEnumerator<Contact>> CreateContactEnumerator(
      BookmarkContactEnumeratorOptions enumeratorOptions)
    {
      throw new NotImplementedException();
    }

    public void RegisterOperation(IXdbOperation operation) => this._operations.Add(operation);

    public async Task SubmitAsync()
    {
      foreach (IXdbOperation operation in this._operations)
        ;
      this._operations.Clear();
    }

    public async Task SubmitAsync(CancellationToken cancellationToken) => await this.SubmitAsync();

    private IEnumerable<TEntity> GetEntities<TEntity>() where TEntity : Entity => this._createdObjects.ContainsKey(typeof (TEntity)) ? this._createdObjects[typeof (TEntity)].Select<Entity, TEntity>((Func<Entity, TEntity>) (v => (TEntity) v)) : Enumerable.Empty<TEntity>();
  }
}
