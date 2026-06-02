namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

/// <summary>
/// Represents a single message in a chat conversation.
/// </summary>
public class ChatMessage
{
    /// <summary>Role of the message author: "user", "assistant", or "system".</summary>
    public string Role { get; set; } = "user";

    /// <summary>The textual content of the message.</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>UTC timestamp when the message was created.</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}