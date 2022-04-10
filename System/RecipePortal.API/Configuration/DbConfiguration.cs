using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Context.Factories;
using RecipePortal.Db.Context.Setup;
using RecipePortal.Settings;

namespace RecipePortal.API.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IApiSettings settings)
    {
        var dbOptionsDelegate = DbContextOptionFactory.Configure(settings.Db.ConnectionString);

        services.AddDbContextFactory<MainDbContext>(dbOptionsDelegate, ServiceLifetime.Singleton);

        return services;
    }

    public static WebApplication UseAppDbContext(this WebApplication app)
    {
        DbInit.Execute(app.Services);

        DbSeed.Execute(app.Services);

        return app;
    }
}
