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

    public static IApplicationBuilder UseAppDbContext(this IApplicationBuilder app)
    {
        DbInit.Execute(app.ApplicationServices);

        DbSeed.Execute(app.ApplicationServices);

        return app;
    }
}
