using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class SubscriptionToAuthorModel
{
    public int SubscriptionId { get; set; }

    public UserAccountModel Author { get; set; }
}

public class SubscriptionToAuthorModelProfile : Profile
{
    public SubscriptionToAuthorModelProfile()
    {
        CreateMap<SubscriptionToAuthor, SubscriptionToAuthorModel>()
            .ForMember(d => d.SubscriptionId, a => a.MapFrom(src => src.Id))
            .ForMember(d => d.Author, a => a.MapFrom(src => src.Author));
    }
}
