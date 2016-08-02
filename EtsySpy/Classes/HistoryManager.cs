using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EtsySpy.Properties;

namespace EtsySpy.Classes
{
    public class HistoryManager
    {
        private int maxHistoryToKeep = 0;

        public HistoryManager()
        {
            maxHistoryToKeep = Int32.Parse(ConfigurationManager.AppSettings["max-history"]);
        }

        public EtsyQuery GetLastQuery()
        {
            if (Settings.Default != null)
            {
                EtsyQuery lastQuery = Settings.Default.LastQuery;

                return lastQuery;
            }
            

            return null;
        }

        public string GetLastQueryText()
        {
            EtsyQuery lastQuery = this.GetLastQuery();
            if (lastQuery != null)
            {
                return lastQuery.QueryText;
            }

            return string.Empty;
        }

        public void SaveLastQuery(string queryText, string friendlyName, EtsyQuery.QueryTypes queryType)
        {
            EtsyQuery lastQuery = new EtsyQuery
            {
                QueryText = queryText,
                FriendlyName = friendlyName,
                QueryType = queryType
            };

            Settings.Default.LastQuery = lastQuery;

            // Also add to query history
            this.SaveQueryToHistory(lastQuery);
        }

        public QueryHistory GetQueryHistory()
        {
            if (Settings.Default != null)
            {
                QueryHistory history = Settings.Default.QueryHistory ?? new QueryHistory();

                return history;
            }


            return null;
        }

        public void SaveQueryToHistory(EtsyQuery query)
        {
            QueryHistory history = this.GetQueryHistory();

            if (!history.History.Contains(query))
            {
                if (history.History.Count > maxHistoryToKeep)
                {
                    // Max history reached, purge oldest
                }

                history.History.Add(query);

                Settings.Default.QueryHistory = history;
            }


        } 
    }
}
