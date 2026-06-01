using Microsoft.AspNetCore.Http;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.SalesApi.Authentication;

public class HeaderSnowflakeCredentialsProvider : ISnowflakeCredentialsProvider
{
    public const string EmailHeader = "X-User-Email";
    public const string PatHeader = "X-Snowflake-PAT";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HeaderSnowflakeCredentialsProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Username => GetHeader(EmailHeader);
    public string Password => GetHeader(PatHeader);

    private string GetHeader(string name)
    {
        var value = _httpContextAccessor.HttpContext?.Request.Headers[name].ToString();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"Required header '{name}' is missing.");
        return value;
    }
}