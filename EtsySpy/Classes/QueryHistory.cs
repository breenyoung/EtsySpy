using System.Collections.Generic;
using System.Configuration;

namespace EtsySpy.Classes
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class QueryHistory
    {
        public List<EtsyQuery> History { get; set; }

        public QueryHistory()
        {
            if (this.History == null)
            {
                this.History = new List<EtsyQuery>();
            }
        }
    }
}