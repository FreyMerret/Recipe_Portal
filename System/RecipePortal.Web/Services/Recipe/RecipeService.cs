using System.Text;
using System.Text.Json;

namespace RecipePortal.Web;

public class RecipeService : IRecipeService
{
    private readonly IAuthService _authService;

    public HttpClient _httpClient { get; }

    public RecipeService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    #region --------------------------------------------------Recipes--------------------------------------------------


    public async Task<List<RecipeListItem>> GetRecipes(string recipeName = "", int categoryId = 0, string authorNickname = "", int offset = 0, int limit = 10)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes?offset={offset}&limit={limit}";

        if (recipeName != "")
            url += $"&recipeName={recipeName}";
        if (categoryId != 0)
            url += $"&categoryId={categoryId}";
        if (authorNickname != "")
            url += $"&authorNickname={authorNickname}";



        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.RefreshJWT();    //Обновляем токен
                return await GetRecipes(recipeName, categoryId, authorNickname, offset, limit);    //уверен, что не будет бесконечной рекурсии потому,
                                                                                                   //что мы или обновим токен или нас выкинет на стр авторизации
            }
            else
                throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<List<RecipeListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RecipeListItem>();

        return data;
    }

    public async Task<RecipeListItem> GetRecipe(int recipeId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.RefreshJWT();
                return await GetRecipe(recipeId); 
            }
            else
                throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<RecipeListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RecipeListItem();

        return data;
    }

    public async Task<RecipeListItem> AddRecipe(AddRecipeRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<RecipeListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RecipeListItem();

        return data;
    }

    public async Task DeleteRecipe(int recipeId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}";

        var response = await _httpClient.DeleteAsync(url);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    #endregion

    //done
    #region -------------------------------------------------Comments--------------------------------------------------

    public async Task<List<CommentListItem>> GetComments(int recipeId, int offset = 0, int limit = 10)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments?offset={offset}&limit={limit}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<List<CommentListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CommentListItem>();

        return data;
    }

    public async Task<CommentListItem> AddComment(int recipeId, CommentRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<CommentListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CommentListItem();

        return data;
    }

    public async Task<CommentListItem> EditComment(int recipeId, int commentId, CommentRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments/{commentId}";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<CommentListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CommentListItem();

        return data;
    }

    public async Task DeleteComment(int recipeId, int commentId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments/{commentId}";

        var response = await _httpClient.DeleteAsync(url);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    #endregion 

    #region ---------------------------------------------CompositionFields---------------------------------------------

    #endregion

    #region -------------------------------------------------Addition--------------------------------------------------

    public async Task<List<CategoryListItem>> GetCategories()
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/categories";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<List<CategoryListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CategoryListItem>();
        return data;
    }

    public async Task<List<IngredientListItem>> GetIngredients()
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/ingredients";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<List<IngredientListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<IngredientListItem>();
        return data;
    }

    #endregion
}
