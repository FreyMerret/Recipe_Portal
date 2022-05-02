using AutoMapper;
using RecipePortal.RecipeService.Models;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class AllSubscriptionsModel
{
    public IEnumerable<SubscriptionToAuthorModel> SubscriptionsToAuthors { get; set; }

    public IEnumerable<SubscriptionToCategoryModel> SubscriptionsToCategories { get; set; }

    public IEnumerable<SubscriptionToCommentsModel> SubscriptionsToComments { get; set; }
}
