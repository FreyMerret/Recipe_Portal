
using AutoMapper;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.UserAccountService.Models;

public class SubscriptionToCommentsModel
{
    public int SubscriptionId { get; set; }

    public ShortRecipeModel Recipe { get; set; }
}

public class SubscriptionToCommentsModelProfile : Profile
{
    public SubscriptionToCommentsModelProfile()
    {
        CreateMap<SubscriptionToComments, SubscriptionToCommentsModel>()
            .ForMember(d => d.SubscriptionId, a => a.MapFrom(src => src.Id))
            .ForMember(d => d.Recipe, a => a.MapFrom(src => src.Recipe));
    }
}
