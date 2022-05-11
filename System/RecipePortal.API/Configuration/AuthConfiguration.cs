namespace RecipePortal.API.Configuration;

using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RecipePortal.Common.Security;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.Settings;
using System.IdentityModel.Tokens.Jwt;

public static class AuthConfiguration
{
    public static IServiceCollection AddAppAuth(this IServiceCollection services, IApiSettings settings)
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

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = settings.IdentityServer.RequireHttps;
                options.Authority = settings.IdentityServer.Url;
                options.TokenValidationParameters = new TokenValidationParameters   //править?
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Audience = "api";
            });


        services.AddAuthorization(options => {
            options.AddPolicy(AppScopes.RecipesRead, policy => policy.RequireClaim("scope", AppScopes.RecipesRead));
            options.AddPolicy(AppScopes.RecipesWrite, policy => policy.RequireClaim("scope", AppScopes.RecipesWrite));
            options.AddPolicy(AppScopes.CommentsRead, policy => policy.RequireClaim("scope", AppScopes.CommentsRead));
            options.AddPolicy(AppScopes.CommentsWrite, policy => policy.RequireClaim("scope", AppScopes.CommentsWrite));
        });

        return services;
    }

    public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}
