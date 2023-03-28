// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.SalesforceDateType
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sitecore.DataExchange.Providers.Salesforce
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SalesforceDateType
  {
    public static readonly Guid DateItemID = new Guid("8F31ADBE-029E-4662-8E84-558AC58D2222");
    public static readonly Guid DateTimeItemID = new Guid("0060E58E-4826-4124-A7F4-52916FCFCCCC");
    public static IDictionary<Guid, string> _salesforceDateType = (IDictionary<Guid, string>) new Dictionary<Guid, string>();

    static SalesforceDateType()
    {
      SalesforceDateType._salesforceDateType[SalesforceDateType.DateItemID] = "Date";
      SalesforceDateType._salesforceDateType[SalesforceDateType.DateTimeItemID] = "DateTime";
    }
  }
}
