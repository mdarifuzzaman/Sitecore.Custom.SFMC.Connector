// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Models.DeviceProfileModel
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.XConnect.Models
{
  public class DeviceProfileModel : EntityModel
  {
    public IEntityReference<Contact> LastKnownContact { get; set; }

    public override Entity AddToClient(
      IXdbContext client,
      IXConnectClientHelper helper,
      Action<IEnumerable<IXdbOperation>, IXdbContext, int, PipelineStep, PipelineContext, ILogger> onOperationsAddedToClient,
      int minBatchSize,
      PipelineStep pipelineStep,
      PipelineContext pipelineContext,
      ILogger logger)
    {
      DeviceProfile deviceProfile = helper.ToDeviceProfile(this);
      AddDeviceProfileOperation profileOperation = client.AddDeviceProfile(deviceProfile);
      onOperationsAddedToClient((IEnumerable<IXdbOperation>) new AddDeviceProfileOperation[1]
      {
        profileOperation
      }, client, minBatchSize, pipelineStep, pipelineContext, logger);
      return (Entity) deviceProfile;
    }
  }
}
