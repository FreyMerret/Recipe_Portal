namespace RecipePortal.Identity;

using RecipePortal.Common.Security;
using Duende.IdentityServer.Models;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(AppScopes.RecipesRead, "Access to recipes API - Read data"),
            new ApiScope(AppScopes.RecipesWrite, "Access to recipes API - Write data"),
            new ApiScope(AppScopes.CommentsRead, "Access to comments API - Read data"),
            new ApiScope(AppScopes.CommentsWrite, "Access to comments API - Write data")
        };
}