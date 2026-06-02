namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels
{
    /// <summary>
    /// Resolves the Snowflake credentials for the current request/user.
    /// </summary>
    public interface ISnowflakeCredentialsProvider
    {
        /// <summary>The Snowflake user account (typically email).</summary>
        string Username { get; }

        /// <summary>The Snowflake programmatic access token (PAT).</summary>
        string Password { get; }
    }
}