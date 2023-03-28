// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ReadObjectsSettings
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.Salesforce.Query;
using System;
using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  public class ReadObjectsSettings : IPlugin
  {
    public ReadObjectsSettings() => this.FieldsToRead = (ICollection<string>) new HashSet<string>();

    public string ObjectName { get; set; }

    public int MaxCount { get; set; }

    public ICollection<string> FieldsToRead { get; private set; }

    public IValueAccessor ObjectIdValueAccessor { get; set; }

    public bool ExcludeUseDeltaSettings { get; set; }

    public FilterExpressionDescriptor FilterExpression { get; set; }

    public Guid QueryLocation { get; set; }

    public Guid ContextObjectLocation { get; set; }

    public string IdentifierFieldName { get; set; }
  }
}
