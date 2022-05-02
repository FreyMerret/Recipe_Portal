using AutoMapper;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;
using RecipePortal.UserAccountService.Models;

namespace RecipePortal.API.Controllers.UserAccounts.Models;
public class SubscriptionToCommentsResponse
{
    public int SubscriptionId { get; set; }

    public ShortRecipeModel Recipe { get; set; }
}

public class SubscriptionToCommentsResponseProfile : Profile
{
    public SubscriptionToCommentsResponseProfile()
    {
        CreateMap<SubscriptionToCommentsModel, SubscriptionToCommentsResponse>();
    }
}
