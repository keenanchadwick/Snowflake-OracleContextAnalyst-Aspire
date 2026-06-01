using Microsoft.AspNetCore.Mvc;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.SalesApi.Controllers;

/// <summary>
/// Provides AI completions backed by SNOWFLAKE.CORTEX.COMPLETE.
/// </summary>
[Route("[controller]")]
[ApiController]
public class AskController : ControllerBase
{
    private readonly ILogger<AskController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AskController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public AskController(ILogger<AskController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sends a prompt (with optional history) to Snowflake Cortex and returns the completion.
    /// </summary>
    [HttpPost(Name = "AskOracle")]
    public async Task<ActionResult<AskResponse>> Post([FromBody] AskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Prompt))
        {
            return BadRequest("Prompt is required.");
        }

        try
        {
            var accessor = new SnowflakeCortexAccessor(Credentials.Username, Credentials.Password);
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