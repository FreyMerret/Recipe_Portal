using RecipePortal.RecipeService.Models;

namespace RecipePortal.RecipeService;

public interface IRecipeService
{
    Task<IEnumerable<RecipeModel>> GetRecipes(int offset = 0, int limit = 10);

    Task<RecipeModel> GetRecipe(int id);

    Task<RecipeModel> AddRecipe(AddRecipeModel model);

    Task UpdateRecipe(int id, UpdateRecipeModel model);

    Task DeleteRecipe(int id);
}
