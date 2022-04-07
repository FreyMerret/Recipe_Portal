namespace RecipePortal.RecipeService;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddRecipeService(this IServiceCollection services)
    {
        services.AddSingleton<IRecipeService, RecipeService>();

        return services;
    }
}
