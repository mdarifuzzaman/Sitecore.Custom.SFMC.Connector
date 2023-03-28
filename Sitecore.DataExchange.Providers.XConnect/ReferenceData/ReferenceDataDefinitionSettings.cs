﻿// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.ReferenceData.ReferenceDataDefinitionSettings
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using System;

namespace Sitecore.DataExchange.Providers.XConnect.ReferenceData
{
  public class ReferenceDataDefinitionSettings : IPlugin
  {
    public string DefinitionTypeName { get; set; }

    public short DefinitionTypeVersion { get; set; }

    public Type CommonDataType { get; set; }

    public Type CultureDataType { get; set; }

    public bool InitializeCommonData { get; set; }

    public bool InitializeCultureData { get; set; }

    public bool ActivateDefinition { get; set; }
  }
}