// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ReadObjects.ObjectReaderContextConverter
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System.Collections.Generic;
using System.Linq;

namespace Sitecore.DataExchange.Providers.Salesforce.ReadObjects
{
  public class ObjectReaderContextConverter : IConverter<ReadObjectsSettings, ObjectReaderContext>
  {
    public virtual bool CanConvert(ReadObjectsSettings source) => true;

    public virtual ConvertResult<ObjectReaderContext> Convert(
      ReadObjectsSettings source)
    {
      return ConvertResult<ObjectReaderContext>.PositiveResult(new ObjectReaderContext(source.ObjectName)
      {
        FieldNames = (IEnumerable<string>) source.FieldsToRead.ToArray<string>(),
        MaxCount = source.MaxCount
      });
    }
  }
}
