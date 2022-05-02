using AutoMapper;
using RecipePortal.UserAccountService.Models;

namespace RecipePortal.API.Controllers.UserAccounts.Models;

public class SubscriptionToAuthorResponse
{
    public int SubscriptionId { get; set; }

    public UserAccountResponse Author { get; set; }
}

public class SubscriptionToAuthorResponseProfile : Profile
{
    public SubscriptionToAuthorResponseProfile()
    {
        CreateMap<SubscriptionToAuthorModel, SubscriptionToAuthorResponse>();
    }
}
