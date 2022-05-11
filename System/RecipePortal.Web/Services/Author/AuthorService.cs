using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace RecipePortal.Web;

public class AuthorService : IAuthorService
{
    private readonly IHttpClientBugCatcher _myHttpClient;
    public AuthorService(IHttpClientBugCatcher myHttpClient)
    {
        _myHttpClient = myHttpClient;
    }

    public async Task<IEnumerable<AuthorListItem>> GetAuthors(string authorNickname = "", int offset = 0, int limit = 20)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts?offset={offset}&limit={limit}";

        if (authorNickname != "")
            url += $"&authorNickname={authorNickname}";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<IEnumerable<AuthorListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AuthorListItem>();

        return data;
    }

    public async Task<AuthorListItem> GetAuthor(string authorNickname)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/{authorNickname}";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<AuthorListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AuthorListItem();

        return data;
    }

    #region -------------------------------------------------Subscriptions-------------------------------------------------

    public async Task<AllSubscriptions> GetSubscriptions()
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/my_subscriptions";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<AllSubscriptions>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AllSubscriptions();

        return data;
    }

    public async Task<SubscriptionToAuthorItem> AddSubscriptionToAuthor(string authorNickname)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/{authorNickname}/subscription";
        var content = await _myHttpClient.PostAsync(url, null);

        var data = JsonSerializer.Deserialize<SubscriptionToAuthorItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SubscriptionToAuthorItem();

        return data;
    }

    public async Task DeleteSubscriptionToAuthor(int subscriptionId)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/unsubscribe/{subscriptionId}";
        await _myHttpClient.DeleteAsync(url);
    }

    public async Task<SubscriptionToCategoryItem> AddSubscriptionToCategory(string categoryId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/categories/{categoryId}/subscribe";
        var content = await _myHttpClient.PostAsync(url, null);

        var data = JsonSerializer.Deserialize<SubscriptionToCategoryItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SubscriptionToCategoryItem();

        return data;
    }

    public async Task DeleteSubscriptionToCategory(int subscriptionId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/categories/unsubscribe/{subscriptionId}";
        await _myHttpClient.DeleteAsync(url);
    }

    public async Task<SubscriptionToCommentsItem> AddSubscriptionToComments(int recipeId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/subscribe";
        var content = await _myHttpClient.PostAsync(url, null);

        var data = JsonSerializer.Deserialize<SubscriptionToCommentsItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SubscriptionToCommentsItem();

        return data;
    }

    public async Task DeleteSubscriptionToComments(int subscriptionId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/unsubscribe/{subscriptionId}";
        await _myHttpClient.DeleteAsync(url);
    }

    #endregion
}
