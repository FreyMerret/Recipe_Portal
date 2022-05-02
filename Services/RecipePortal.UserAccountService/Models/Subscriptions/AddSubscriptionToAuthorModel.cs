using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class AddSubscriptionToAuthorModel
{
    public Guid SubscriberId { get; set; }
    public Guid AuthorId { get; set; }
}

public class AddSubscriptionToAuthorModelProfile : Profile
{
    public AddSubscriptionToAuthorModelProfile()
    {
        CreateMap<AddSubscriptionToAuthorModel, SubscriptionToAuthor>();
    }
}
