namespace RecipePortal.Web;

public interface IRecipeService
{
    //RECIPES
    Task<List<RecipeListItem>> GetRecipes(string recipeName = "", int categoryId = 0, string authorNickname = "", int offset = 0, int limit = 10);

    Task<RecipeListItem> GetRecipe(int recipeId);

    Task<RecipeListItem> AddRecipe(AddRecipeRequest model);

    Task<RecipeListItem> UpdateRecipe(int recipeId, UpdateRecipeRequest model);

    Task DeleteRecipe(int recipeId);


    //COMMENTS
    Task<List<CommentListItem>> GetComments(int recipeId, int offset, int limit);

    Task<CommentListItem> AddComment(int recipeId, CommentRequest model);

    Task<CommentListItem> UpdateComment(int recipeId, int commentId, CommentRequest model);

    Task DeleteComment(int recipeId, int commentId);


    //COMPOSITION FIELDS
    Task<CompositionFieldItem> AddCompositionField(int recipeId, CompositionFieldRequest model);

    Task<CompositionFieldItem> UpdateCompositionField(int recipeId, int compositionFieldId, CompositionFieldRequest model);

    Task DeleteCompositionField(int recipeId, int compositionFieldId);


    //ADDITION

    Task<List<CategoryListItem>> GetCategories();

    Task<List<IngredientListItem>> GetIngredients();
}
