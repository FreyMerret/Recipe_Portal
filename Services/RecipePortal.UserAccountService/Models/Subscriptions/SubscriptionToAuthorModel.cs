using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class SubscriptionToAuthorModel
{
    public Guid SubscriberId { get; set; }
    public Guid AuthorId { get; set; }
}

public class SubscriptionToAuthorModelProfile : Profile
{
    public SubscriptionToAuthorModelProfile()
    {
        CreateMap<SubscriptionToAuthorModel, SubscriptionToAuthor>();
    }
}
