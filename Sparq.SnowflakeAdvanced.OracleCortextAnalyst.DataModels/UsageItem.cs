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
        public string? UserName { get; set; }
        /// <summary>
        /// Gets or sets the query text.
        /// </summary>
        /// <value>
        /// The query text.
        /// </value>
        public string? QueryText { get; set; }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public string? Model { get; set; }
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
        /// <summary>
        /// Gets or sets the prompt tokens.
        /// </summary>
        /// <value>
        /// The prompt tokens.
        /// </value>
        public int PromptTokens { get; set; }
        /// <summary>
        /// Gets or sets the completion tokens.
        /// </summary>
        /// <value>
        /// The completion tokens.
        /// </value>
        public int CompletionTokens { get; set; }
        /// <summary>
        /// Gets or sets the total tokens.
        /// </summary>
        /// <value>
        /// The total tokens.
        /// </value>
        public int TotalTokens { get; set; }
        /// <summary>
        /// Gets or sets the token credits.
        /// </summary>
        /// <value>
        /// The token credits.
        /// </value>
        public int TokenCredits { get; set; }
        /// <summary>
        /// Gets or sets the search results count.
        /// </summary>
        /// <value>
        /// The search results count.
        /// </value>
        public int SearchResultsCount { get; set; }
    }
}
