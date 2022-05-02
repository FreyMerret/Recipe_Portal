namespace RecipePortal.Web;

public interface IRecipeService
{
    //RECIPES
    //Task<IEnumerable<RecipeListItem>> GetRecipes(int offset, int limit);

    Task<List<RecipeListItem>> GetRecipes(string recipeName = "", int categoryId = 0, string authorNickname = "", int offset = 0, int limit = 10);

    Task<RecipeListItem> GetRecipe(int recipeId);

    Task<RecipeListItem> AddRecipe(AddRecipeRequest model);

    //Task<RecipeModel> UpdateRecipe(UpdateRecipeModel model);

    Task DeleteRecipe(int recipeId);


    //COMMENTS
    Task<List<CommentListItem>> GetComments(int recipeId, int offset, int limit);

    Task<CommentListItem> AddComment(int recipeId, CommentRequest model);

    Task<CommentListItem> EditComment(int recipeId, int commentId, CommentRequest model);

    Task DeleteComment(int recipeId, int commentId);


    //COMPOSITION FIELDS
    //Task<CompositionFieldModel> AddCompositionField(AddCompositionFieldModel model);

    //Task<CompositionFieldModel> UpdateCompositionField(UpdateCompositionFieldModel model);

    //Task DeleteCompositionField(int compositionFieldId, Guid requestAuthor);


    //ADDITION

    Task<List<CategoryListItem>> GetCategories();

    Task<List<IngredientListItem>> GetIngredients();
}
