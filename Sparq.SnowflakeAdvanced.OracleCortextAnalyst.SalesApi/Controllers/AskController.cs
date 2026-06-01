using Microsoft.AspNetCore.Mvc;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.SalesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AskController : ControllerBase
    {
        private readonly ILogger<AskController> _logger;
        private readonly ISnowflakeCredentialsProvider _credentials;

        public AskController(ILogger<AskController> logger, ISnowflakeCredentialsProvider credentials)
        {
            _logger = logger;
            _credentials = credentials;
        }

        [HttpPost(Name = "AskOracle")]
        public async Task<ActionResult<AskResponse>> Post([FromBody] AskRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.Prompt))
                return BadRequest("Prompt is required.");

            try
            {
                var accessor = new SnowflakeCortexAccessor(_credentials.Username, _credentials.Password);
                var response = await accessor.CompleteAsync(request, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Snowflake Cortex call failed for model {Model}", request.Model);
                return Problem($"Snowflake Cortex call failed: {ex.Message}");
            }
        }
    }
}