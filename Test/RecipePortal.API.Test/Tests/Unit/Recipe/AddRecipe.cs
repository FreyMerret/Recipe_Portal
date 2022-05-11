namespace RecipePortal.API.Test.Tests.Unit.Recipe;

using RecipePortal.RecipeService;
using NUnit.Framework;
using System.Threading.Tasks;

[TestFixture]
public partial class RecipeUnitTest
{
    [Test]
    public async Task AddRecipe_ValidParameters_Success()
    {
        var recipeService = services.Get<IRecipeService>();

        var authorId = await GetExistedAuthorId();
        var ingredientId = await GetExistedIngredientId();
        var categoryId = await GetExistedCategoryId();

        var model = AddRecipeModel(authorId, categoryId, "test", "test", "test", ingredientId, "test");

        var resultRecipe = await recipeService.AddRecipe(model, false);

        Assert.IsNotNull(resultRecipe);

        Assert.AreEqual(model.CategoryId, resultRecipe.CategoryId);
        Assert.AreEqual(model.Title, resultRecipe.Title);
        Assert.AreEqual(model.Text, resultRecipe.Text);
    }
}
