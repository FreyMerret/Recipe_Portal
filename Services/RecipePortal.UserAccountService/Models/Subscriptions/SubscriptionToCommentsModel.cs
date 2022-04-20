using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class SubscriptionToCommentsModel
{
    public Guid SubscriberId { get; set; }
    public int RecipeId { get; set; }
}

public class SubscriptionToCommentsModelProfile : Profile
{
    public SubscriptionToCommentsModelProfile()
    {
        CreateMap<SubscriptionToCommentsModel, SubscriptionToComments>();
    }
}
