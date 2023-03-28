// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.SalesforceDateTypeFormat
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System.Runtime.InteropServices;

namespace Sitecore.DataExchange.Providers.Salesforce
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SalesforceDateTypeFormat
  {
    public static readonly string DateFormat = "yyyy-MM-dd";
    public static readonly string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
  }
}
