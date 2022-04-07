namespace RecipePortal.API;

using RecipePortal.Settings;
using RecipePortal.RecipeService;
//using DSRNetSchool.BookService;
public static class Bootstrapper
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services
            .AddSettings()
            .AddRecipeService();
    }
}
