using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snowflake.Data.Client;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess
{
    /// <summary>
    /// Data accessor for retrieving <see cref="Document"/> records from Snowflake.
    /// </summary>
    public class DocumentsDataAccessor : DataAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsDataAccessor"/> class.
        /// </summary>
        /// <param name="userAccount">The Snowflake user account.</param>
        /// <param name="password">The PAT or password.</param>
        public DocumentsDataAccessor(string userAccount, string password)
            : base(userAccount, password)
        {
        }

        /// <summary>
        /// Gets all documents from SALES_INTELLIGENCE.CORE.DOCUMENTS.
        /// </summary>
        public async Task<IReadOnlyList<Document>> GetDocumentsAsync()
        {
            var documents = new List<Document>();

            using var connection = new SnowflakeDbConnection { ConnectionString = ConnectionString };
            await connection.OpenAsync().ConfigureAwait(false);

            using var command = connection.CreateCommand();
            command.CommandText = Queries.GetDocuments;

            using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                documents.Add(new Document
                {
                    FileName = reader.IsDBNull(0) ? null : reader.GetString(0),
                    FileType = reader.IsDBNull(1) ? null : reader.GetString(1),
                    FileSize = reader.IsDBNull(2) ? (long?)null : reader.GetInt64(2),
                    RawText = reader.IsDBNull(3) ? null : reader.GetString(3),
                    CleanedText = reader.IsDBNull(4) ? null : reader.GetString(4),
                    LoadedAt = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                    ContentHash = reader.IsDBNull(6) ? null : reader.GetString(6),
                });
            }

            return documents;
        }
    }
}