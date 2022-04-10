namespace RecipePortal.Identity.Configuration;

using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Context.Factories;
using RecipePortal.Settings;
using Microsoft.Extensions.DependencyInjection;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IDbSettings settings)
    {
        var dbOptionsDelegate = DbContextOptionFactory.Configure(settings.ConnectionString);

        services.AddDbContextFactory<MainDbContext>(dbOptionsDelegate, ServiceLifetime.Singleton);

        return services;
    }

    public static IApplicationBuilder UseAppDbContext(this IApplicationBuilder app)
    {
        return app;
    }
}
