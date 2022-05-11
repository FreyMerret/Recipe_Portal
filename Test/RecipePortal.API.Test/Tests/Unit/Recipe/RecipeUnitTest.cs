namespace RecipePortal.API.Test.Tests.Unit.Recipe;

using RecipePortal.API.Test.Common;
using RecipePortal.Db.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class RecipeUnitTest : UnitTest
{
    public async Task<Guid> GetExistedAuthorId()
    {
        await using var context = await DbContext();

        var author = context.Users.AsEnumerable().First();
        return author.Id;
    }

    public async Task<int> GetExistedCategoryId()
    {
        await using var context = await DbContext();

        var category = context.Categories.AsEnumerable().First();
        return category.Id;
    }

    public async Task<int> GetExistedIngredientId()
    {
        await using var context = await DbContext();

        var category = context.Ingredients.AsEnumerable().First();
        return category.Id;
    }

    public async Task<int> GetExistedRecipeId()
    {
        await using var context = await DbContext();

        var recipe = context.Recipes.AsEnumerable().First();
        return recipe.Id;
    }


    public async Task<Recipe> GetRecipeById(int id)
    {
        await using var context = await DbContext();
        var recipe = context.Recipes.FirstOrDefault(x => x.Id == id);
        return recipe;
    }

    protected async override Task ClearDb()
    {
        await using var context = await DbContext();
        context.Recipes.RemoveRange(context.Recipes);
        context.Users.RemoveRange(context.Users);
        context.SaveChanges();
    }
}
