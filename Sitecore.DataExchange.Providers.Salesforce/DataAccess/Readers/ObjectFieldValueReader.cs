// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers.ObjectFieldValueReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using System;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers
{
  public class ObjectFieldValueReader : IValueReader
  {
    public ObjectFieldValueReader(string fieldName)
    {
      this.FieldName = fieldName;
      this.InternalValueReader = (IValueReader) new DictionaryValueReader<string, object>(fieldName);
    }

    public string FieldName { get; private set; }

    protected IValueReader InternalValueReader { get; set; }

    public ReadResult Read(object source, DataAccessContext context) => this.InternalValueReader == null ? ReadResult.NegativeResult(DateTime.Now) : this.InternalValueReader.Read(source, context);
  }
}
