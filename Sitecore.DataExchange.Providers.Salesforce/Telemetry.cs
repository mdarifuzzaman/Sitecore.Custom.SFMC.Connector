// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Telemetry
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.Diagnostics;
using Sitecore.Nexus.Consumption;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce
{
  internal static class Telemetry
  {
    private const string ConnectorsSfCrmInstallMetric = "Connectors.SF.CRM.Install|FLAG|41IFoJKrpgEvOjJsMMSpSbN+dPQ6d6M1mMVf70qJ0RtuyD0sJUILDQm00Awu/C5diYzKN7WWpGK3F+/JSwKM5g==";
    private const string ConnectorsSfCrmSfCampaignsToXConnectSyncMetric = "Connectors.SF.CRM.SfCampaignsToXConnectSync.Execute|SUM|UYdgn+NfMdghw4PbC6xZTBW3kBF+eIVUFurQgoGA4fJn84Iyh7rLhUQGF/tU/TkOGAyN0PmLTiQVfv5lhGafHg==";
    private const string ConnectorsSfCrmSfContactsToXConnectSyncMetric = "Connectors.SF.CRM.SfContactsToXConnectSync.Execute|SUM|1JjTqy1ovd3sqplDnaNz+VfGrmcuHpERe8LyUGFiInyUUoYS5ORulkDLI3y5ewBPLnBAWtQF5UX2CirB4E49ew==";
    private const string ConnectorsSfCrmSfTasksToXConnectSyncMetric = "Connectors.SF.CRM.SfTasksToXConnectSync.Execute|SUM|TGjt1RbxqT3qk90AjrX6JIpIJFK0HY7guPaqcdqZBD/k0WZbtgHXcN/Kx1dgN0omRp43p31+edBoSd28iKkIxg==";
    private const string ConnectorsSfCrmXConnectContactsToSfSyncMetric = "Connectors.SF.CRM.XConnectContactsToSfSync.Execute|SUM|hcp0M9kABP29pJMZLFARWchE58x6JnFjgUbLG5dmcnLxlNoT37lyhuoqg6WzyIWuCdmrLsA8kxyJecL5nkBL5Q==";

    private static TelemetryClient Client { get; } = TelemetryFactory.CreateClient();

    internal static void TrackConnectorsSfCrmInstall() => Telemetry.Track("Connectors.SF.CRM.Install|FLAG|41IFoJKrpgEvOjJsMMSpSbN+dPQ6d6M1mMVf70qJ0RtuyD0sJUILDQm00Awu/C5diYzKN7WWpGK3F+/JSwKM5g==");

    internal static void TrackConnectorsSfCrmSfCampaignsToXConnectSyncExecute() => Telemetry.Track("Connectors.SF.CRM.SfCampaignsToXConnectSync.Execute|SUM|UYdgn+NfMdghw4PbC6xZTBW3kBF+eIVUFurQgoGA4fJn84Iyh7rLhUQGF/tU/TkOGAyN0PmLTiQVfv5lhGafHg==");

    internal static void TrackConnectorsSfCrmSfContactsToXConnectSyncExecute() => Telemetry.Track("Connectors.SF.CRM.SfContactsToXConnectSync.Execute|SUM|1JjTqy1ovd3sqplDnaNz+VfGrmcuHpERe8LyUGFiInyUUoYS5ORulkDLI3y5ewBPLnBAWtQF5UX2CirB4E49ew==");

    internal static void TrackConnectorsSfCrmSfTasksToXConnectSyncExecute() => Telemetry.Track("Connectors.SF.CRM.SfTasksToXConnectSync.Execute|SUM|TGjt1RbxqT3qk90AjrX6JIpIJFK0HY7guPaqcdqZBD/k0WZbtgHXcN/Kx1dgN0omRp43p31+edBoSd28iKkIxg==");

    internal static void TrackConnectorsSfCrmXConnectContactsToSfSyncExecute() => Telemetry.Track("Connectors.SF.CRM.XConnectContactsToSfSync.Execute|SUM|hcp0M9kABP29pJMZLFARWchE58x6JnFjgUbLG5dmcnLxlNoT37lyhuoqg6WzyIWuCdmrLsA8kxyJecL5nkBL5Q==");

    private static void Track(string metric)
    {
      try
      {
        Telemetry.Client.Track(metric, 1UL);
      }
      catch (Exception ex)
      {
        Log.Error("[SFCRM] Telemetry Error.", ex, ex.GetType());
      }
    }
  }
}
