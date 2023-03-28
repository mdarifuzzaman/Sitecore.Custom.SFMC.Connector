// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ApiUsage.ApiUsageDetails
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

namespace Sitecore.DataExchange.Providers.Salesforce.ApiUsage
{
  public class ApiUsageDetails
  {
    public ApiUsageDetails(int limit, int remaining)
    {
      this.Limit = limit;
      this.Remaining = remaining;
      this.Used = limit - remaining;
    }

    public int Limit { get; private set; }

    public int Remaining { get; private set; }

    public int Used { get; private set; }
  }
}
