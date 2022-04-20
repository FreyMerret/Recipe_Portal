namespace RecipePortal.Worker;

using RecipePortal.EmailService;
using RecipePortal.RabbitMqService;
using RecipePortal.Settings;
using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .AddSettings()
            .AddEmailSender()
            .AddRabbitMq()            
            .AddSingleton<ITaskExecutor, TaskExecutor>();

        return services;
    }
}
 



