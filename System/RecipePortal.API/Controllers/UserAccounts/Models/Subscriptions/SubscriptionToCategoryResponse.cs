using AutoMapper;
using RecipePortal.RecipeService.Models;
using RecipePortal.UserAccountService.Models;

namespace RecipePortal.API.Controllers.UserAccounts.Models;
public class SubscriptionToCategoryResponse
{
    public int SubscriptionId { get; set; }

    public CategoryModel Category { get; set; }
}

public class SubscriptionToCategoryResponseProfile : Profile
{
    public SubscriptionToCategoryResponseProfile()
    {
        CreateMap<SubscriptionToCategoryModel, SubscriptionToCategoryResponse>();
    }
}

