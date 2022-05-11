namespace RecipePortal.Identity.Configuration;

using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.Identity.Configuration.IS4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Settings;

public static class IS4Configuration
{
    public static IServiceCollection AddIS4(this IServiceCollection services, bool? testUsers = false)
    {
        services
            .AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 0;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders();

        if (testUsers == true)  //костыль, чтобы работали тесты
            services
            .AddIdentityServer()
            .AddAspNetIdentity<User>()
            .AddInMemoryApiScopes(AppApiScopes.ApiScopes)
            .AddInMemoryClients(AppClients.Clients)
            .AddInMemoryApiResources(AppResources.Resources)
            .AddInMemoryIdentityResources(AppIdentityResources.Resources)

            .AddTestUsers(AppApiTestUsers.ApiUsers)

            .AddDeveloperSigningCredential();
        else
            services
            .AddIdentityServer()
            .AddAspNetIdentity<User>()
            .AddInMemoryApiScopes(AppApiScopes.ApiScopes)
            .AddInMemoryClients(AppClients.Clients)
            .AddInMemoryApiResources(AppResources.Resources)
            .AddInMemoryIdentityResources(AppIdentityResources.Resources)
            .AddDeveloperSigningCredential();

        return services;
    }

    public static IApplicationBuilder UseIS4(this IApplicationBuilder app)
    {
        app.UseIdentityServer();

        return app;
    }
}
