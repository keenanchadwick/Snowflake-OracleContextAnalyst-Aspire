using System.Net.Http.Json;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web;

public class SalesApiClient(HttpClient httpClient)
{
    /// <summary>
    /// Gets the query usage asynchronously.
    /// </summary>
    public async Task<UsageItem[]> GetQueryUsageAsync(CancellationToken cancellationToken = default)
    {
        var items = await httpClient.GetFromJsonAsync<UsageItem[]>("/QueryUsage", cancellationToken);
        return items ?? [];
    }

    /// <summary>
    /// Sends a chat prompt (with optional history) to Snowflake Cortex via the SalesApi.
    /// </summary>
    public async Task<AskResponse> AskAsync(AskRequest request, CancellationToken cancellationToken = default)
    {
        // Snowflake Cortex calls can take a while; raise the timeout for chat scenarios.
        using var response = await httpClient.PostAsJsonAsync("/Ask", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AskResponse>(cancellationToken: cancellationToken);
        return result ?? new AskResponse();
    }
}