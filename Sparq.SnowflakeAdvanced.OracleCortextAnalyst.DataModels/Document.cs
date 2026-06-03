using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels
{
    /// <summary>
    /// Defines the structure of a document loaded into the system, including metadata and text content for analysis.
    /// </summary>
    public class Document
    {
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
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long? FileSize { get; set; }
        /// <summary>
        /// Gets or sets the raw text.
        /// </summary>
        /// <value>
        /// The raw text.
        /// </value>
        public string RawText { get; set; }
        /// <summary>
        /// Gets or sets the cleaned text.
        /// </summary>
        /// <value>
        /// The cleaned text.
        /// </value>
        public string CleanedText { get; set; }
        /// <summary>
        /// Gets or sets the loaded at.
        /// </summary>
        /// <value>
        /// The loaded at.
        /// </value>
        public DateTime? LoadedAt { get; set; }
        /// <summary>
        /// Gets or sets the content hash.
        /// </summary>
        /// <value>
        /// The content hash.
        /// </value>
        public string ContentHash { get; set; }
    }
}
