using RecipePortal.RecipeService.Models;

namespace RecipePortal.RecipeService;

public interface IRecipeService
{
    //RECIPES
    Task<IEnumerable<RecipeModel>> GetRecipes(string recipeName = "", int categoryId = 0, string authorNickname = "", int offset = 0, int limit = 10);

    Task<RecipeModel> GetRecipe(int recipeId);

    Task<RecipeModel> AddRecipe(AddRecipeModel model);

    Task<RecipeModel> UpdateRecipe(int recipeId, UpdateRecipeModel model);

    Task DeleteRecipe(int recipeId);


    //COMPOSITION FIELDS
    Task<CompositionFieldModel> AddCompositionField(int recipeId, AddCompositionFieldModel model);

    Task<CompositionFieldModel> UpdateCompositionField(int compositionFieldId, UpdateCompositionFieldModel model);

    Task DeleteCompositionField(int compositionFieldId);


    //COMMENTS
    Task<IEnumerable<CommentModel>> GetComments(int recipeId, int offset = 0, int limit = 10);

    Task<CommentModel> AddComment(int recipeId, AddCommentModel model);

    Task<CommentModel> UpdateComment(int commentId, UpdateCommentModel model);

    Task DeleteComment(int commentId);
}
