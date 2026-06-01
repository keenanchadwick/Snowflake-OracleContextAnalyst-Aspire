using Microsoft.AspNetCore.Mvc;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.SalesApi.Controllers
{
    /// <summary>
    /// Defines the functionality for access to the query usage log table in Snowflake
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("[controller]")]
    [ApiController]
    public class QueryUsageController : ControllerBase
    {
        private readonly ILogger<QueryUsageController> _logger;
        private readonly ISnowflakeCredentialsProvider _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryUsageController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="credentials">The credentials provider.</param>
        public QueryUsageController(ILogger<QueryUsageController> logger, ISnowflakeCredentialsProvider credentials)
        {
            _logger = logger;
            _credentials = credentials;
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{UsageItem}"/> of recent query usage records.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetQueryUsage")]
        public IEnumerable<UsageItem> Get()
        {
            var accessor = new UsageLogAccessor(_credentials.Username, _credentials.Password);
            return accessor.GetRecentUsage();
        }
    }
}
