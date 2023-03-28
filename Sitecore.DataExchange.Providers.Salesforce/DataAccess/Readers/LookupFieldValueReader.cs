using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.DataAccess.Readers
{
    public class LookupFieldValueReader : IValueReader
    {
        public LookupFieldValueReader(string propertyName) => this.PropertyName = propertyName;

        public string PropertyName { get; private set; }

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (!this.CanRead(source, context))
                return ReadResult.NegativeResult(DateTime.Now);
            IDictionary<string, object> dictionary1 = source as IDictionary<string, object>;
            if (!dictionary1.ContainsKey("records") || !(dictionary1["records"] is IEnumerable<object> objects))
                return ReadResult.NegativeResult(DateTime.Now);
            List<string> readValue = new List<string>();
            foreach (IDictionary<string, object> dictionary2 in objects)
            {
                foreach (string key in (IEnumerable<string>)dictionary2.Keys)
                {
                    if (key.Equals(this.PropertyName, StringComparison.OrdinalIgnoreCase))
                        readValue.Add(dictionary2[key].ToString());
                }
            }
            return ReadResult.PositiveResult((object)readValue.Aggregate((i, j) => i + " " + j) , DateTime.Now);
        }

        protected virtual bool CanRead(object source, DataAccessContext context) => source is IDictionary<string, object>;
    }
}
