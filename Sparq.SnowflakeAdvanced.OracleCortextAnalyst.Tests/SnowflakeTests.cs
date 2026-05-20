using Snowflake.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Tests;

[TestClass]
public class SnowflakeTests
{
    /// <summary>Gets or sets the test context.</summary>
    /// <value>The test context.</value>
    public TestContext TestContext { get; set; }

    const string _connectionString = "account=SPARQPARTNER-AZWUS2;user=KEENAN.CHADWICK@TEAMSPARQ.COM;authenticator=externalbrowser;db=SALES_INTELLIGENCE;schema=CORE";

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
}