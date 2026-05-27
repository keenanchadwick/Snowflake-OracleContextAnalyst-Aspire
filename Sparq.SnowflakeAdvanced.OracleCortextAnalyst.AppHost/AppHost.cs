var builder = DistributedApplication.CreateBuilder(args);

var salesApiService = builder.AddProject<Projects.Sparq_SnowflakeAdvanced_OracleCortextAnalyst_SalesApi>("salesapi")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Sparq_SnowflakeAdvanced_OracleCortextAnalyst_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(salesApiService)
    .WaitFor(salesApiService);

builder.Build().Run();
