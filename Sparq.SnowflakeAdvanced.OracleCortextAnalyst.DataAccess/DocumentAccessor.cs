using System;
using System.Collections.Generic;
using System.Data.Common;
using Snowflake.Data.Client;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess
{
    /// <summary>
    /// Provides read access to the SALES_INTELLIGENCE.CORE.DOCUMENTS and
    /// SALES_INTELLIGENCE.CORE.DOCUMENT_CHUNKS tables in Snowflake.
    /// </summary>
    /// <seealso cref="DataAccessor" />
    public class DocumentAccessor : DataAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAccessor"/> class.
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="password">The password, usually the programmatic access token (PAT).</param>
        public DocumentAccessor(string userAccount, string password) : base(userAccount, password) { }

        /// <summary>
        /// Gets the full set of documents from Snowflake.
        /// </summary>
        public IEnumerable<Document> GetDocuments()
        {
            List<Document> items = new List<Document>();

            using (SnowflakeDbConnection connection = new SnowflakeDbConnection(ConnectionString))
            {
                using (SnowflakeDbCommand command = new SnowflakeDbCommand(connection, Queries.GetDocuments))
                {
                    connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Document item = new Document
                            {
                                FileName = Convert.ToString(reader["FILE_NAME"]),
                                FileType = Convert.ToString(reader["FILE_TYPE"]),
                                RawText = Convert.ToString(reader["RAW_TEXT"]),
                                CleanedText = Convert.ToString(reader["CLEANED_TEXT"]),
                                ContentHash = Convert.ToString(reader["CONTENT_HASH"])
                            };

                            if (long.TryParse(Convert.ToString(reader["FILE_SIZE"]), out var size)) { item.FileSize = size; }
                            if (DateTime.TryParse(Convert.ToString(reader["LOADED_AT"]), out var loaded)) { item.LoadedAt = loaded; }

                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Gets the cleaned text segments for a single document, ordered by chunk index.
        /// </summary>
        /// <param name="fileName">The document file name to scope the chunks to.</param>
        public IEnumerable<DocumentChunk> GetChunksForDocument(string fileName)
        {
            List<DocumentChunk> items = new List<DocumentChunk>();
            if (string.IsNullOrWhiteSpace(fileName)) { return items; }

            // Reuse the resource query and filter client-side to avoid touching the .resx for a one-off WHERE clause.
            using (SnowflakeDbConnection connection = new SnowflakeDbConnection(ConnectionString))
            {
                using (SnowflakeDbCommand command = new SnowflakeDbCommand(connection, Queries.GetDocumentChunks))
                {
                    connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = Convert.ToString(reader["FILE_NAME"]);
                            if (!string.Equals(name, fileName, StringComparison.OrdinalIgnoreCase)) { continue; }

                            DocumentChunk item = new DocumentChunk
                            {
                                FileName = name,
                                FileType = Convert.ToString(reader["FILE_TYPE"]),
                                RawTextSegment = Convert.ToString(reader["RAW_TEXT_SEGMENT"]),
                                CleanedTextSegment = Convert.ToString(reader["CLEANED_TEXT_SEGMENT"]),
                                SectionContext = Convert.ToString(reader["SECTION_CONTEXT"])
                            };

                            if (long.TryParse(Convert.ToString(reader["CHUNK_ID"]), out var chunkId)) { item.ChunkId = chunkId; }
                            if (long.TryParse(Convert.ToString(reader["CHUNK_INDEX"]), out var chunkIndex)) { item.ChunkIndex = chunkIndex; }

                            items.Add(item);
                        }
                    }
                }
            }

            items.Sort((a, b) => a.ChunkIndex.CompareTo(b.ChunkIndex));
            return items;
        }
    }
}