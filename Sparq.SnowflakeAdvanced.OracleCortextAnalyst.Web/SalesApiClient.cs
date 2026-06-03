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

    /// <summary>
    /// Gets the full set of documents available for browsing in the document library.
    /// </summary>
    public async Task<Document[]> GetDocumentsAsync(CancellationToken cancellationToken = default)
    {
        var items = await httpClient.GetFromJsonAsync<Document[]>("/Documents", cancellationToken);
        return items ?? [];
    }

    /// <summary>
    /// Sends a question scoped to a single document to the SalesApi.
    /// </summary>
    public async Task<AskResponse> AskDocumentAsync(string fileName, AskRequest request, CancellationToken cancellationToken = default)
    {
        var encoded = Uri.EscapeDataString(fileName);
        using var response = await httpClient.PostAsJsonAsync($"/Documents/{encoded}/ask", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AskResponse>(cancellationToken: cancellationToken);
        return result ?? new AskResponse();
    }
}