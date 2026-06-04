using Microsoft.AspNetCore.Mvc;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.SalesApi.Controllers
{
    /// <summary>
    /// Defines the functionality for browsing the documents library and asking
    /// questions scoped to a single document.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly ISnowflakeCredentialsProvider _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="credentials">The credentials provider.</param>
        public DocumentsController(ILogger<DocumentsController> logger, ISnowflakeCredentialsProvider credentials)
        {
            _logger = logger;
            _credentials = credentials;
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{Document}"/> of all documents available for browsing.
        /// </summary>
        [HttpGet(Name = "GetDocuments")]
        public IEnumerable<Document> Get()
        {
            var accessor = new DocumentAccessor(_credentials.Username, _credentials.Password);
            return accessor.GetDocuments();
        }

        /// <summary>
        /// Asks a question scoped to a single document by injecting its chunks
        /// as context into the existing Snowflake Cortex completion call.
        /// </summary>
        /// <param name="fileName">The document file name to scope the answer to.</param>
        /// <param name="request">The chat request from the caller.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpPost("{fileName}/ask", Name = "AskDocument")]
        public async Task<ActionResult<AskResponse>> AskAsync(
            string fileName,
            [FromBody] AskRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("fileName is required.");
            if (string.IsNullOrWhiteSpace(request?.Prompt))
                return BadRequest("Prompt is required.");

            try
            {
                var docs = new DocumentAccessor(_credentials.Username, _credentials.Password);
                var chunks = docs.GetChunksForDocument(fileName).ToList();

                if (chunks.Count == 0)
                {
                    return NotFound($"No chunks found for document '{fileName}'.");
                }

                var context = string.Join("\n\n", chunks
                    .Select(c => string.IsNullOrWhiteSpace(c.CleanedTextSegment) ? c.RawTextSegment : c.CleanedTextSegment)
                    .Where(s => !string.IsNullOrWhiteSpace(s)));

                // Wrap the user's prompt with the scoped document context so the existing
                // Cortex accessor can answer using only the supplied document.
                var scopedRequest = new AskRequest
                {
                    Model = request.Model,
                    History = request.History,
                    Prompt =
                        $"You are answering ONLY using the following document context for '{fileName}'. " +
                        "If the answer is not contained in the context, say you don't know.\n\n" +
                        "=== DOCUMENT CONTEXT START ===\n" +
                        context +
                        "\n=== DOCUMENT CONTEXT END ===\n\n" +
                        "Question: " + request.Prompt
                };

                var cortex = new SnowflakeCortexAccessor(_credentials.Username, _credentials.Password);
                var response = await cortex.CompleteAsync(scopedRequest, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Scoped document ask failed for {FileName}", fileName);
                return Problem($"Scoped document ask failed: {ex.Message}");
            }
        }
    }
}