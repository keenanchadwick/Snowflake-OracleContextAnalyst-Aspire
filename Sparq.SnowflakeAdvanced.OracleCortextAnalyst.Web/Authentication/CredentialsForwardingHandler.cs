using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web.Authentication;

public class CredentialsForwardingHandler : DelegatingHandler
{
    public const string EmailHeader = "X-User-Email";
    public const string PatHeader = "X-Snowflake-PAT";

    private readonly ISnowflakeCredentialsProvider _credentials;

    public CredentialsForwardingHandler(ISnowflakeCredentialsProvider credentials)
    {
        _credentials = credentials;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Remove(EmailHeader);
        request.Headers.Remove(PatHeader);
        request.Headers.Add(EmailHeader, _credentials.Username);
        request.Headers.Add(PatHeader, _credentials.Password);
        return base.SendAsync(request, cancellationToken);
    }
}