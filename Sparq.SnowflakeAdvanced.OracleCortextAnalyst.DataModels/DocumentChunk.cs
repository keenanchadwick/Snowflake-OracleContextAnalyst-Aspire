using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels
{
    /// <summary>
    /// Defines a chunk of a document, including its text segments, context, and embedding vector for AI processing.
    /// </summary>
    public class DocumentChunk
    {
        /// <summary>
        /// Gets or sets the chunk identifier.
        /// </summary>
        /// <value>
        /// The chunk identifier.
        /// </value>
        public long ChunkId { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public string FileType { get; set; }
        /// <summary>
        /// Gets or sets the index of the chunk.
        /// </summary>
        /// <value>
        /// The index of the chunk.
        /// </value>
        public long ChunkIndex { get; set; }
        /// <summary>
        /// Gets or sets the raw text segment.
        /// </summary>
        /// <value>
        /// The raw text segment.
        /// </value>
        public string RawTextSegment { get; set; }
        /// <summary>
        /// Gets or sets the cleaned text segment.
        /// </summary>
        /// <value>
        /// The cleaned text segment.
        /// </value>
        public string CleanedTextSegment { get; set; }
        /// <summary>
        /// Gets or sets the section context.
        /// </summary>
        /// <value>
        /// The section context.
        /// </value>
        public string SectionContext { get; set; }
        /// <summary>
        /// Gets or sets the embedding vector.
        /// </summary>
        /// <value>
        /// The embedding vector.
        /// </value>
        public float[] EmbeddingVector { get; set; }
    }
}
