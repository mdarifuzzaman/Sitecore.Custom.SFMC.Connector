// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Client.CredentialsWebRequestHandlerModifierOptions
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System.Data.Common;

namespace Sitecore.DataExchange.Providers.XConnect.Client
{
  public class CredentialsWebRequestHandlerModifierOptions
  {
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Mode { get; set; }

    public static CredentialsWebRequestHandlerModifierOptions Parse(
      string connectionString)
    {
      CredentialsWebRequestHandlerModifierOptions handlerModifierOptions = new CredentialsWebRequestHandlerModifierOptions();
      if (!string.IsNullOrWhiteSpace(connectionString))
      {
        DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder()
        {
          ConnectionString = connectionString
        };
        object obj = (object) null;
        if (connectionStringBuilder.TryGetValue("Mode", out obj))
          handlerModifierOptions.Mode = (string) obj;
        if (connectionStringBuilder.TryGetValue("UserName", out obj))
          handlerModifierOptions.UserName = (string) obj;
        if (connectionStringBuilder.TryGetValue("Password", out obj))
          handlerModifierOptions.Password = (string) obj;
      }
      return handlerModifierOptions;
    }
  }
}
