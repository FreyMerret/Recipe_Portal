using RecipePortal.Common.Helpers;

namespace RecipePortal.API.Configuration;

public static class AutoMapperConfiguration
{
    public static IServiceCollection AddAutoMappers(this IServiceCollection services)
    {
        AutoMappersRegisterHelper.Register(services);

        return services;
    }
}
