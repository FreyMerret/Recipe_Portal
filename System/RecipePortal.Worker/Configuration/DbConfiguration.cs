using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Context.Factories;
using RecipePortal.Settings;

namespace RecipePortal.Worker;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IWorkerSettings settings)
    {
        var dbOptionsDelegate = DbContextOptionFactory.Configure(settings.Db.ConnectionString);

        services.AddDbContextFactory<MainDbContext>(dbOptionsDelegate, ServiceLifetime.Singleton);

        return services;
    }

    public static WebApplication UseAppDbContext(this WebApplication app)
    {
        return app;
    }
}
