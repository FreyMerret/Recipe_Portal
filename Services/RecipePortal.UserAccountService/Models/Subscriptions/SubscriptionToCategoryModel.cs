using AutoMapper;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.UserAccountService.Models;

public class SubscriptionToCategoryModel
{
    public int SubscriptionId { get; set; }

    public CategoryModel Category { get; set; }
}

public class SubscriptionToCategoryModelProfile : Profile
{
    public SubscriptionToCategoryModelProfile()
    {
        CreateMap<SubscriptionToCategory, SubscriptionToCategoryModel>()
            .ForMember(d => d.SubscriptionId, a => a.MapFrom(src => src.Id))
            .ForMember(d => d.Category, a => a.MapFrom(src => src.Category));
    }
}

