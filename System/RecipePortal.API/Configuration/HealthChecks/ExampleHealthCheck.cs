namespace RecipePortal.API.Configuration;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

public class ExampleHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
    {
        var assembly = Assembly.Load("RecipePortal.API");
        var versionNumber = assembly.GetName().Version;

        return HealthCheckResult.Healthy(description: $"Build {versionNumber}");
    }
}