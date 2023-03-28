// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels.ReferencedXdbFacetDefinitionConverter
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Repositories;

namespace Sitecore.DataExchange.Providers.XConnect.Converters.CollectionModels
{
  [SupportedIds(new string[] {"{31AD5E1A-AC6D-4EC4-A284-86BB10729DFA}"})]
  public class ReferencedXdbFacetDefinitionConverter : BaseXdbFacetDefinitionConverter
  {
    public ReferencedXdbFacetDefinitionConverter(IItemModelRepository repository)
      : base(repository)
    {
    }
  }
}
