// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers.EventsFromInteractionValueReader
// Assembly: Sitecore.DataExchange.Providers.XConnect, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79449A24-60CA-40C6-A977-3401990E2F4B
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.XConnect.dll

using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.XConnect.Expressions;
using Sitecore.XConnect;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecore.DataExchange.Providers.XConnect.DataAccess.Readers
{
    public class EventsFromInteractionValueReader : IValueReader
    {
        public readonly IEventExpressionBuilder EventExpressionBuilder;

        public EventsFromInteractionValueReader(IEventExpressionBuilder eventExpressionBuilder) => this.EventExpressionBuilder = eventExpressionBuilder;

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            ReadResult result = null;
            if (source == null || !(source is Interaction interaction))
                return ReadResult.NegativeResult(DateTime.Now);
            Expression<Func<Event, bool>> predicate = this.EventExpressionBuilder?.Build(new ExpressionContext());
            if (predicate == null)
            {
                result = ReadResult.PositiveResult((object)interaction.Events, DateTime.Now);
            }
            else
            {
                result = ReadResult.PositiveResult((object)interaction.Events.AsQueryable<Event>().Where<Event>(predicate), DateTime.Now);
            }
            return result;
        }
    }
}
