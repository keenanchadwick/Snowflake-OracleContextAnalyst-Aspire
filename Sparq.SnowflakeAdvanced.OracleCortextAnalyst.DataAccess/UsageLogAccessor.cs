using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;
using Snowflake.Data.Client;
using System.Data.Common;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess
{
    public class UsageLogAccessor : DataAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsageLogAccessor"/> class.
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="password">The password, usually the programmatic access token (PAT).</param>
        public UsageLogAccessor(string userAccount, string password) : base(userAccount, password) { }

        /// <summary>
        /// Gets the recent usage.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsageItem> GetRecentUsage()
        {
            List<UsageItem> items = new List<UsageItem>();

            using (SnowflakeDbConnection connection = new SnowflakeDbConnection(ConnectionString))
            {
                using (SnowflakeDbCommand command = new SnowflakeDbCommand(connection, Queries.GetQueryUsage))
                {
                    connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UsageItem item = new UsageItem
                            {
                                UserName = Convert.ToString(reader["USER_NAME"]),
                                QueryText = Convert.ToString(reader["QUERY_TEXT"]),
                                Model = Convert.ToString(reader["MODEL"])
                            };

                            DateTime testDate;
                            if (DateTime.TryParse(Convert.ToString(reader["LOGGED_AT"]), out testDate)) { item.LoggedAt = testDate; }

                            int testInt;
                            if (int.TryParse(Convert.ToString(reader["SEARCH_LATENCY_MS"]), out testInt)) { item.SearchLatencyMS = testInt; }
                            if (int.TryParse(Convert.ToString(reader["COMPLETE_LATENCY_MS"]), out testInt)) { item.CompleteLatencyMS = testInt; }
                            if (int.TryParse(Convert.ToString(reader["PROMPT_TOKENS"]), out testInt)) { item.PromptTokens = testInt; }
                            if (int.TryParse(Convert.ToString(reader["COMPLETION_TOKENS"]), out testInt)) { item.CompletionTokens = testInt; }
                            if (int.TryParse(Convert.ToString(reader["TOTAL_TOKENS"]), out testInt)) { item.TotalTokens = testInt; }
                            if (int.TryParse(Convert.ToString(reader["TOKEN_CREDITS"]), out testInt)) { item.TokenCredits = testInt; }
                            if (int.TryParse(Convert.ToString(reader["SEARCH_RESULTS_COUNT"]), out testInt)) { item.SearchResultsCount = testInt; }

                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }
    }
}