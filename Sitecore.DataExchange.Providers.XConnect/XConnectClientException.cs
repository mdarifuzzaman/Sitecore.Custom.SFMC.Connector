// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.XConnectClientException
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System;
using System.Runtime.Serialization;

namespace Sitecore.DataExchange.Providers.XConnect
{
  public class XConnectClientException : Exception
  {
    public XConnectClientException()
    {
    }

    public XConnectClientException(string message)
      : base(message)
    {
    }

    public XConnectClientException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public XConnectClientException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
