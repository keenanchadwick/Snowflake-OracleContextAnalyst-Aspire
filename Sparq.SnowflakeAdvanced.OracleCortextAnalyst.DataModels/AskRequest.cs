namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

/// <summary>
/// Request payload sent to the Snowflake Cortex completion endpoint.
/// </summary>
public class AskRequest
{
    /// <summary>The user's prompt text.</summary>
    public string Prompt { get; set; } = string.Empty;

    /// <summary>Optional Snowflake Cortex model to invoke (e.g. "mistral-large2").</summary>
    public string Model { get; set; } = "mistral-large2";

    /// <summary>Optional prior conversation turns to provide context.</summary>
    public List<ChatMessage> History { get; set; } = new();
}