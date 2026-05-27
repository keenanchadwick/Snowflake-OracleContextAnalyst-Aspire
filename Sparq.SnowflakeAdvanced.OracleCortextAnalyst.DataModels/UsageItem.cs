namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels
{
    /// <summary>
    /// Defines the structure of the query usage log records
    /// </summary>
    public class UsageItem
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the query text.
        /// </summary>
        /// <value>
        /// The query text.
        /// </value>
        public string QueryText { get; set; }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public string Model { get; set; }
        /// <summary>
        /// Gets or sets the logged at.
        /// </summary>
        /// <value>
        /// The logged at.
        /// </value>
        public DateTime LoggedAt { get; set; }
        /// <summary>
        /// Gets or sets the search latency in milliseconds.
        /// </summary>
        /// <value>
        /// The search latency in milliseconds.
        /// </value>
        public int SearchLatencyMS { get; set; }
        /// <summary>
        /// Gets or sets the complete latency in milliseconds.
        /// </summary>
        /// <value>
        /// The complete latency in milliseconds.
        /// </value>
        public int CompleteLatencyMS { get; set; }
    }
}
