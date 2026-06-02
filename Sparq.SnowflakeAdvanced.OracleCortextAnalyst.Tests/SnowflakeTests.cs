using Snowflake.Data.Client;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Tests;

[TestClass]
public class SnowflakeTests
{
    /// <summary>Gets or sets the test context.</summary>
    /// <value>The test context.</value>
    public TestContext TestContext { get; set; }

    static readonly string _connectionString =
        String.Format("account=SPARQPARTNER-AZWUS2;user={0};db=SALES_INTELLIGENCE;schema=CORE;password={1}", Credentials.Username, Credentials.Password);

    /*select user_name, query_text, model, logged_at, search_latency_ms, complete_latency_ms
from SALES_INTELLIGENCE.CORE.QUERY_USAGE_LOG order by logged_at desc*/

    [TestMethod]
    public void TestConnection()
    {
        using(SnowflakeDbConnection connection = new SnowflakeDbConnection(_connectionString))
        {
            using(SnowflakeDbCommand command = new SnowflakeDbCommand(connection, "SHOW DATABASES"))
            {
                connection.Open();

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string databaseName = reader.GetString(1);
                        TestContext.WriteLine(databaseName);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Tests the usage log accessor.
    /// </summary>
    [TestMethod]
    public void TestUsageLogAccessor()
    {
        UsageLogAccessor accessor = new UsageLogAccessor(Credentials.Username, Credentials.Password);
        var usageItems = accessor.GetRecentUsage();

        Assert.IsNotNull(usageItems);
        Assert.IsTrue(usageItems.Any(), "Expected to find at least one usage item.");
    }
}