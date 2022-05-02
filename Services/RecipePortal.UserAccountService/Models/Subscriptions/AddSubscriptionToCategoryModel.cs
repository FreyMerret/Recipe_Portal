using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class AddSubscriptionToCategoryModel
{
    public Guid SubscriberId { get; set; }
    public int CategoryId { get; set; }
}

public class AddSubscriptionToCategoryModelProfile : Profile
{
    public AddSubscriptionToCategoryModelProfile()
    {
        CreateMap<AddSubscriptionToCategoryModel, SubscriptionToCategory>();
    }
}
