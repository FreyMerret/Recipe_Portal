using AutoMapper;
using RecipePortal.API.Controllers.Recipes.Models;
using RecipePortal.UserAccountService.Models;

namespace RecipePortal.API.Controllers.UserAccounts.Models;

public class AllSubscriptionsResponse
{
    public IEnumerable<SubscriptionToAuthorResponse> SubscriptionsToAuthors { get; set; }

    public IEnumerable<SubscriptionToCategoryResponse> SubscriptionsToCategories { get; set; }

    public IEnumerable<SubscriptionToCommentsResponse> SubscriptionsToComments { get; set; }
}

public class AllSubscriptionsResponseProfile : Profile
{
    public AllSubscriptionsResponseProfile()
    {
        CreateMap<AllSubscriptionsModel, AllSubscriptionsResponse>()
            .ForMember(d => d.SubscriptionsToAuthors, a => a.MapFrom(src => src.SubscriptionsToAuthors))
            .ForMember(d => d.SubscriptionsToCategories, a => a.MapFrom(src => src.SubscriptionsToCategories))
            .ForMember(d => d.SubscriptionsToComments, a => a.MapFrom(src => src.SubscriptionsToComments));
    }
}
