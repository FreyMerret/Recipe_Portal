using System.Text;
using System.Text.Json;

namespace RecipePortal.Web;

public class RecipeService : IRecipeService
{
    private readonly IHttpClientBugCatcher _myHttpClient;
    public RecipeService(IHttpClientBugCatcher myHttpClient)
    {
        _myHttpClient = myHttpClient;
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

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<List<RecipeListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RecipeListItem>();

        return data;
    }

    public async Task<RecipeListItem> GetRecipe(int recipeId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<RecipeListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RecipeListItem();

        return data;
    }

    public async Task<RecipeListItem> AddRecipe(AddRecipeRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var content = await _myHttpClient.PostAsync(url, request);

        var data = JsonSerializer.Deserialize<RecipeListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RecipeListItem();

        return data;
    }

    public async Task<RecipeListItem> UpdateRecipe(int recipeId, UpdateRecipeRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var content = await _myHttpClient.PutAsync(url, request);

        var data = JsonSerializer.Deserialize<RecipeListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RecipeListItem();

        return data;
    }

    public async Task DeleteRecipe(int recipeId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}";

        await _myHttpClient.DeleteAsync(url);
    }

    #endregion

    #region -------------------------------------------------Comments--------------------------------------------------

    public async Task<List<CommentListItem>> GetComments(int recipeId, int offset = 0, int limit = 10)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments?offset={offset}&limit={limit}";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<List<CommentListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CommentListItem>();

        return data;
    }

    public async Task<CommentListItem> AddComment(int recipeId, CommentRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var content = await _myHttpClient.PostAsync(url, request);

        var data = JsonSerializer.Deserialize<CommentListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CommentListItem();

        return data;
    }

    public async Task<CommentListItem> UpdateComment(int recipeId, int commentId, CommentRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments/{commentId}";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var content = await _myHttpClient.PutAsync(url, request);

        var data = JsonSerializer.Deserialize<CommentListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CommentListItem();

        return data;
    }

    public async Task DeleteComment(int recipeId, int commentId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/comments/{commentId}";

        await _myHttpClient.DeleteAsync(url);
    }

    #endregion

    #region ---------------------------------------------CompositionFields---------------------------------------------

    public async Task<CompositionFieldItem> AddCompositionField(int recipeId, CompositionFieldRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/ingredients";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var content = await _myHttpClient.PostAsync(url, request);

        var data = JsonSerializer.Deserialize<CompositionFieldItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CompositionFieldItem();

        return data;
    }

    public async Task<CompositionFieldItem> UpdateCompositionField(int recipeId, int compositionFieldId, CompositionFieldRequest model)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/ingredients/{compositionFieldId}";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var content = await _myHttpClient.PutAsync(url, request);

        var data = JsonSerializer.Deserialize<CompositionFieldItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CompositionFieldItem();

        return data;
    }

    public async Task DeleteCompositionField(int recipeId, int compositionFieldId)
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/{recipeId}/ingredients/{compositionFieldId}";

        await _myHttpClient.DeleteAsync(url);
    }

    #endregion

    #region -------------------------------------------------Addition--------------------------------------------------

    public async Task<List<CategoryListItem>> GetCategories()
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/categories";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<List<CategoryListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CategoryListItem>();
        return data;
    }

    public async Task<List<IngredientListItem>> GetIngredients()
    {
        string url = $"{Settings.ApiRoot}/v1/recipes/ingredients";

        var content = await _myHttpClient.GetAsync(url);

        var data = JsonSerializer.Deserialize<List<IngredientListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<IngredientListItem>();
        return data;
    }

    #endregion
}
