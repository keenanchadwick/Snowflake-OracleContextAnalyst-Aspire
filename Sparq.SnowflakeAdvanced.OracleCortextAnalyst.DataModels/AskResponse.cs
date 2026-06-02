namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

/// <summary>
/// Response payload from the Snowflake Cortex completion endpoint.
/// </summary>
public class AskResponse
{
    /// <summary>The assistant's reply text.</summary>
    public string Reply { get; set; } = string.Empty;

    /// <summary>The model that produced the reply.</summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>Round-trip latency in milliseconds.</summary>
    public long ElapsedMs { get; set; }
}