using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class AddSubscriptionToCommentsModel
{
    public Guid SubscriberId { get; set; }
    public int RecipeId { get; set; }
}

public class AddSubscriptionToCommentsModelProfile : Profile
{
    public AddSubscriptionToCommentsModelProfile()
    {
        CreateMap<AddSubscriptionToCommentsModel, SubscriptionToComments>();
    }
}
