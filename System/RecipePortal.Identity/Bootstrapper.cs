using RecipePortal.Settings;

namespace RecipePortal.Identity
{
    public static class Bootstrapper
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services
                .AddSettings();
        }
    }
}
