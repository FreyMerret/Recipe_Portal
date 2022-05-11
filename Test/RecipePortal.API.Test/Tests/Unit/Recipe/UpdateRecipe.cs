namespace RecipePortal.API.Test.Tests.Unit.Recipe;

using RecipePortal.RecipeService;
using NUnit.Framework;
using System.Threading.Tasks;

[TestFixture]
public partial class RecipeUnitTest
{
    [Test]
    public async Task UpdateRecipe_ValidParameters_Success()
    {
        var recipeService = services.Get<IRecipeService>();

        var recipeId = await GetExistedRecipeId();
        var authorId = await GetExistedAuthorId();
        var categoryId = await GetExistedCategoryId();

        var model = UpdateRecipeModel(authorId, recipeId, categoryId, "test", "test", "test");

        await recipeService.UpdateRecipe(model);

        var resultRecipe = await GetRecipeById(recipeId);

        Assert.IsNotNull(resultRecipe);

        Assert.AreEqual(model.CategoryId, resultRecipe.CategoryId);
        Assert.AreEqual(model.Title, resultRecipe.Title);
        Assert.AreEqual(model.Text, resultRecipe.Text);
    }
}
