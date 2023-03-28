// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ReadObjectsByIdentifierStepProcessor
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.Services.Core.Diagnostics;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  [RequiredPipelineContextPlugins(new Type[] {typeof (SynchronizationSettings)})]
  [RequiredPipelineStepPlugins(new Type[] {typeof (ResolveIdentifierSettings)})]
  public class ReadObjectsByIdentifierStepProcessor : ReadObjectsStepProcessor
  {
    protected virtual object GetIdentifierObject(
      Endpoint endpoint,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      return this.GetObjectFromPipelineContext(pipelineStep.GetResolveIdentifierSettings().IdentifierObjectLocation, pipelineContext, logger);
    }

    protected virtual object ReadIdentifierValue(
      object identifierObject,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext)
    {
      IValueAccessor identifierValueAccessor = pipelineStep.GetResolveIdentifierSettings().IdentifierValueAccessor;
      if (identifierValueAccessor != null)
      {
        IValueReader valueReader = identifierValueAccessor.ValueReader;
        if (valueReader != null)
        {
          ReadResult readResult = valueReader.Read(identifierObject, new DataAccessContext());
          if (readResult.WasValueRead)
            return readResult.ReadValue;
        }
      }
      return (object) null;
    }
  }
}
