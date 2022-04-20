namespace RecipePortal.API;

using RecipePortal.Settings;
using RecipePortal.RecipeService;
using RecipePortal.EmailService;
using RecipePortal.RabbitMqService;
using RecipePortal.UserAccountService;

public static class Bootstrapper
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services
            .AddSettings()
            .AddRecipeService()
            .AddEmailSender()
            .AddRabbitMq()
            .AddUserAccountService();
    }
}
