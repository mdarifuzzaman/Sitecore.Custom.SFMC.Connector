// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification.DummyXConnectClientConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Providers.XConnect.Client;
using Sitecore.XConnect;

namespace Sitecore.DataExchange.Providers.XConnect.PipelineBatchVerification
{
  public class DummyXConnectClientConverter : XConnectClientConverter
  {
    public override ConvertResult<IXdbContext> Convert(
      XConnectClientSettings clientSettings)
    {
      ConvertResult<IXdbContext> convertResult = base.Convert(clientSettings);
      return !convertResult.WasConverted ? ConvertResult<IXdbContext>.NegativeResult("The source object could not be converted.") : ConvertResult<IXdbContext>.PositiveResult((IXdbContext) new DummyXConnectClient(convertResult.ConvertedValue));
    }
  }
}
