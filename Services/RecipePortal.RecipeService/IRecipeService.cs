using RecipePortal.RecipeService.Models;

namespace RecipePortal.RecipeService;

public interface IRecipeService
{
    //RECIPES
    Task<IEnumerable<RecipeModel>> GetRecipes(string recipeName = "", int categoryId = 0, string authorNickname = "", int offset = 0, int limit = 10);

    Task<RecipeModel> GetRecipe(int recipeId);

    Task<RecipeModel> AddRecipe(AddRecipeModel model);

    Task<RecipeModel> UpdateRecipe(UpdateRecipeModel model);

    Task DeleteRecipe(int recipeId, Guid requestAuthor);


    //COMMENTS
    Task<IEnumerable<CommentModel>> GetComments(int recipeId, int offset = 0, int limit = 10);

    Task<CommentModel> AddComment(AddCommentModel model);

    Task<CommentModel> UpdateComment(UpdateCommentModel model);

    Task DeleteComment(int commentId, Guid requestAuthor);


    //COMPOSITION FIELDS
    Task<CompositionFieldModel> AddCompositionField(AddCompositionFieldModel model);

    Task<CompositionFieldModel> UpdateCompositionField(UpdateCompositionFieldModel model);

    Task DeleteCompositionField(int compositionFieldId, Guid requestAuthor);


    //ADDITION

    Task<IEnumerable<CategoryModel>> GetCategories();

    Task<IEnumerable<IngredientModel>> GetIngredients();
}
