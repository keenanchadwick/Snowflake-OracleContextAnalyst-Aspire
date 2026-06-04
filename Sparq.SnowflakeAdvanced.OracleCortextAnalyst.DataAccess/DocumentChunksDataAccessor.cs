using System.Threading.Tasks;
using Snowflake.Data.Client;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess
{
    /// <summary>
    /// Data accessor for asking Cortex questions scoped to a single document.
    /// </summary>
    public class DocumentChunksDataAccessor : DataAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentChunksDataAccessor"/> class.
        /// </summary>
        public DocumentChunksDataAccessor(string userAccount, string password)
            : base(userAccount, password)
        {
        }

        /// <summary>
        /// Asks Cortex a question using only the chunks of the supplied document as context.
        /// </summary>
        /// <param name="fileName">FILE_NAME used to scope the chunk lookup.</param>
        /// <param name="question">The user question.</param>
        /// <param name="model">The Cortex model identifier (e.g. "mistral-large2").</param>
        public async Task<string> AskScopedQuestionAsync(string fileName, string question, string model = "mistral-large2")
        {
            // Pull only the cleaned text segments for the given file, then feed them to CORTEX.COMPLETE
            const string scopedSql = @"
WITH ctx AS (
    SELECT LISTAGG(CLEANED_TEXT_SEGMENT, '\n\n') WITHIN GROUP (ORDER BY CHUNK_INDEX) AS CONTEXT_TEXT
    FROM SALES_INTELLIGENCE.CORE.DOCUMENT_CHUNKS
    WHERE FILE_NAME = ?
)
SELECT SNOWFLAKE.CORTEX.COMPLETE(
    ?,
    ARRAY_CONSTRUCT(
        OBJECT_CONSTRUCT('role','system','content',
            'You are a Sales Operations assistant. Answer ONLY using the provided document context. If the answer is not present, say you don''t know.'),
        OBJECT_CONSTRUCT('role','user','content',
            'Document context:\n' || ctx.CONTEXT_TEXT || '\n\nQuestion: ' || ?)
    ),
    OBJECT_CONSTRUCT('temperature', 0.2, 'max_tokens', 1024)
) AS ANSWER
FROM ctx;";

            using var connection = new SnowflakeDbConnection { ConnectionString = ConnectionString };
            await connection.OpenAsync().ConfigureAwait(false);

            using var command = connection.CreateCommand();
            command.CommandText = scopedSql;

            var pFile = command.CreateParameter(); pFile.ParameterName = "1"; pFile.Value = fileName;
            var pModel = command.CreateParameter(); pModel.ParameterName = "2"; pModel.Value = model;
            var pQ = command.CreateParameter(); pQ.ParameterName = "3"; pQ.Value = question;
            command.Parameters.Add(pFile);
            command.Parameters.Add(pModel);
            command.Parameters.Add(pQ);

            var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
            return result?.ToString() ?? string.Empty;
        }
    }
}