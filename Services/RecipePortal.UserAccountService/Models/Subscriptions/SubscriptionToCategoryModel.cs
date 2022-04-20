using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class SubscriptionToCategoryModel
{
    public Guid SubscriberId { get; set; }
    public int CategoryId { get; set; }
}

public class SubscriptionToCategoryModelProfile : Profile
{
    public SubscriptionToCategoryModelProfile()
    {
        CreateMap<SubscriptionToCategoryModel, SubscriptionToCategory>();
    }
}
