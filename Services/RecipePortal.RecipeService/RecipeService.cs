using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Common.Exeptions;
using RecipePortal.Common.Validator;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.RecipeService;

internal class RecipeService : IRecipeService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<AddRecipeModel> addRecipeModelValidator;
    private readonly IModelValidator<UpdateRecipeModel> updateRecipeModelValidator;

    public RecipeService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<AddRecipeModel> addRecipeModelValidator,
        IModelValidator<UpdateRecipeModel> updateRecipeModelValidator)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.addRecipeModelValidator = addRecipeModelValidator;
        this.updateRecipeModelValidator = updateRecipeModelValidator;
    }

    //-------------------------------------------------------------------------------------------------------------

    public async Task< IEnumerable<RecipeModel>> GetRecipes(int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipes = context.Recipes.AsQueryable();

        recipes = recipes //.where()
            .Skip(Math.Max(offset, 0))
            .Take(Math.Min(limit, 100));

        var data = (await recipes.ToListAsync()).Select(recipe => mapper.Map<RecipeModel>(recipe));

        return data;
    }

    public async Task<RecipeModel> GetRecipe(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(id));
        //Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = mapper.Map<RecipeModel>(recipe);

        return data;
    }

    public async Task<RecipeModel> AddRecipe(AddRecipeModel model)
    {
        addRecipeModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = mapper.Map<Recipe>(model);
        await context.Recipes.AddAsync(recipe);
        context.SaveChanges();

        return mapper.Map<RecipeModel>(recipe);
    }

    public async Task UpdateRecipe(int id, UpdateRecipeModel model)
    {
        updateRecipeModelValidator.Check(model);


        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(id))
            ?? throw new ProcessException($"The recipe (id: {id}) was not found");

        recipe = mapper.Map(model, recipe);

        context.Recipes.Update(recipe);
        context.SaveChanges();
    }

    public async Task DeleteRecipe(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(id))
            ?? throw new ProcessException($"The recipe (id: {id}) was not found");

        context.Remove(recipe);
        context.SaveChanges();
    }
}
