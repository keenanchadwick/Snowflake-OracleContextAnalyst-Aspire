using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web.Authentication;

/// <summary>
/// Reads Snowflake credentials from the authenticated user's claims.
/// </summary>
public class ClaimsSnowflakeCredentialsProvider : ISnowflakeCredentialsProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsSnowflakeCredentialsProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Username =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
        ?? throw new InvalidOperationException("User is not authenticated.");

    public string Password =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(SnowflakeClaimTypes.PersonalAccessToken)
        ?? throw new InvalidOperationException("Snowflake PAT claim is missing.");
}