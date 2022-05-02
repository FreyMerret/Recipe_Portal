
namespace RecipePortal.Web;

public class AllSubscriptions
{
    public List<SubscriptionToAuthorItem> SubscriptionsToAuthors { get; set; }

    public List<SubscriptionToCategoryItem> SubscriptionsToCategories { get; set; }

    public List<SubscriptionToCommentsItem> SubscriptionsToComments { get; set; }
}

