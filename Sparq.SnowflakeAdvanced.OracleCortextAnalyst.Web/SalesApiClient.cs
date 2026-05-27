using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web;

public class SalesApiClient(HttpClient httpClient)
{
    /// <summary>
    /// Gets the query usage asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<UsageItem[]> GetQueryUsageAsync(CancellationToken cancellationToken = default)
    {
        var items = await httpClient.GetFromJsonAsync<UsageItem[]>("/QueryUsage", cancellationToken);
        return items ?? [];
    }
}