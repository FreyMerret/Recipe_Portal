namespace RecipePortal.Web;

public interface IAuthorService
{
    Task<IEnumerable<AuthorListItem>> GetAuthors(string authorNickname, int offset, int limit);

    Task<AuthorListItem> GetAuthor(string authorNickname);

    Task<AllSubscriptions> GetSubscriptions();

    Task<SubscriptionToAuthorItem> AddSubscriptionToAuthor(string authorNickname);
    
    Task DeleteSubscriptionToAuthor(int subscriptionId);

    Task<SubscriptionToCategoryItem> AddSubscriptionToCategory(string categoryId);

    Task DeleteSubscriptionToCategory(int subscriptionId);

    Task<SubscriptionToCommentsItem> AddSubscriptionToComments(int recipeId);

    Task DeleteSubscriptionToComments(int subscriptionId);
}
