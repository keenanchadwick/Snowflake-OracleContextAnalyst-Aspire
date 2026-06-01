using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess
{
    /// <summary>
    /// Base class for all data access functionality in the application.
    /// This class is responsible for managing the connection string and providing common functionality for derived classes.
    /// </summary>
    public abstract class DataAccessor
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessor" /> class.
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="password">The password, usually the programmatic access token (PAT).</param>
        protected DataAccessor(string userAccount, string password)
        {
            const string connectionStringTemplate = "account=SPARQPARTNER-AZWUS2;user={0};password={1};db=SALES_INTELLIGENCE;schema=CORE";
            ConnectionString = string.Format(connectionStringTemplate, userAccount, password);
        }
    }
}
