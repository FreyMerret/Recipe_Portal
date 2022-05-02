using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace RecipePortal.Web;

public class AuthorService : IAuthorService
{
    public HttpClient _httpClient { get; }
    public IAuthService _authService { get; }

    public AuthorService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<IEnumerable<AuthorListItem>> GetAuthors(string authorNickname = "", int offset = 0, int limit = 20)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts?offset={offset}&limit={limit}";

        if (authorNickname != "")
            url += $"&authorNickname={authorNickname}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<AuthorListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AuthorListItem>();

        return data;
    }

    public async Task<AuthorListItem> GetAuthor(string authorNickname)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/{authorNickname}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<AuthorListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AuthorListItem();

        return data;
    }

    #region -------------------------------------------------Subscriptions-------------------------------------------------

    public async Task<AllSubscriptions> GetSubscriptions()
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/my_subscriptions";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.RefreshJWT();    //Обновляем токен
                return await GetSubscriptions();    //уверен, что не будет бесконечной рекурсии потому, что мы или обновим токен или нас выкинет на стр авторизации
            }
            else
             throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<AllSubscriptions>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AllSubscriptions();

        return data;
    }

    public async Task<SubscriptionToAuthorItem> AddSubscriptionToAuthor(string authorNickname)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/{authorNickname}/subscription";
        var response = await _httpClient.PostAsync(url,null);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.RefreshJWT();        //Обновляем токен
                return await AddSubscriptionToAuthor(authorNickname);
            }
            else
                throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<SubscriptionToAuthorItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SubscriptionToAuthorItem();

        return data;
    }

    public async Task DeleteSubscriptionToAuthor(int subscriptionId)
    {
        string url = $"{Settings.ApiRoot}/v1/accounts/unsubscribe/{subscriptionId}";
        var response = await _httpClient.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task<SubscriptionToCategoryItem> AddSubscriptionToCategory(string categoryId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/categories/{categoryId}/subscribe";
        var response = await _httpClient.PostAsync(url, null);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<SubscriptionToCategoryItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SubscriptionToCategoryItem();

        return data;
    }

    public async Task DeleteSubscriptionToCategory(int subscriptionId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/categories/unsubscribe/{subscriptionId}";
        var response = await _httpClient.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task<SubscriptionToCommentsItem> AddSubscriptionToComments(int recipeId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/subscribe";
        var response = await _httpClient.PostAsync(url, null);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<SubscriptionToCommentsItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SubscriptionToCommentsItem();

        return data;
    }

    public async Task DeleteSubscriptionToComments(int subscriptionId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/unsubscribe/{subscriptionId}";
        var response = await _httpClient.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    #endregion
}
