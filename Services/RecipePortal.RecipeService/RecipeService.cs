using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Common.Exeptions;
using RecipePortal.Common.Validator;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;
using static RecipePortal.RecipeService.Models.RecipeModel;

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

    public async Task<IEnumerable<RecipeModel>> GetRecipes(int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipes = context
            .Recipes
            .Include(x => x.Composition)   //подтягиваю данные из связанной таблицы для последующего мапинга
            .Include(x => x.Category)
            .Include(x => x.Author)
            .AsQueryable();



        recipes = recipes //.where()
            .Skip(Math.Max(offset, 0))
            .Take(Math.Min(limit, 100));

        var data = (await recipes.ToListAsync()).Select(recipe => mapper.Map<RecipeModel>(recipe)).ToList();

        //получили 

        //var compositions = context
        //    .Compositions
        //    .Include(x => x.Ingredient)
        //    .AsQueryable();

        //foreach (var recipe in data)
        //{
        //    compositions = compositions
        //        .Where(x => x.RecipeId.Equals(recipe.Id));

        //    recipe.Composition = new List<CompositionField>();

        //    foreach (var i in compositions)
        //    {
        //        recipe.Composition.Add(new CompositionField()
        //        {
        //            IngredientId = i.IngredientId,
        //            IngredientName = i.Ingredient.Name,
        //            Quantity = i.Quantity
        //        });
        //    };
        //}
        //var result = (IEnumerable<RecipeModel>)data;
        
        return data;
    }

    public async Task<RecipeModel> GetRecipe(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes
            .Include(x => x.Composition)   //подтягиваю данные из связанной таблицы для последующего мапинга
            .Include(x => x.Category)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = mapper.Map<RecipeModel>(recipe);

        var compositions = context
            .Compositions
            .Include(x => x.Ingredient)
            .Where(x => x.RecipeId.Equals(id))
            .AsQueryable();                

        var comp = new List<CompositionField>();

        foreach (var i in compositions)
        {
            comp.Add(new CompositionField()
            {
                IngredientId = i.IngredientId,
                IngredientName = i.Ingredient.Name,
                Quantity = i.Quantity
            });
        };

        data.Composition = comp;

        return data;
    }

    public async Task<RecipeModel> AddRecipe(AddRecipeModel model)
    {
        // addRecipeModelValidator.Check(model); -----сделать
        // заменить == на Equal

        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = mapper.Map<Recipe>(model);

        ////выбираем все ингридиенты из строки и находим их в бд, формируем коллекцию и приставем ее рецепту
        //ICollection<Ingredient> ingredients = new List<Ingredient>();
        //string[] input_ingredients = model.Ingredients.Split(",");
        //foreach (string ingredient in input_ingredients)
        //    ingredients.Add(await context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredient));
        //recipe.Ingredients = ingredients;

        ////находим автора в бд и пристваиаем его рецепту, затем присваем рецепту guid автора
        //recipe.Author = await context.Users.FirstOrDefaultAsync(i => i.UserName == model.Author);
        ////recipe.AuthorId = recipe.Author.Id;

        ////находим категорию в бд и присваиваем ее рецепту
        //recipe.Category = await context.Categories.FirstOrDefaultAsync(i => i.Id == model.CategoryId);

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
