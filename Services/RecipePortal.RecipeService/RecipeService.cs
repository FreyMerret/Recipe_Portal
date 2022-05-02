using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Common.Exeptions;
using RecipePortal.Common.Validator;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.RabbitMqService;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.RecipeService;

public class RecipeService : IRecipeService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IRabbitMqTask rabbitMqTask;
    private readonly IModelValidator<AddRecipeModel> addRecipeModelValidator;
    private readonly IModelValidator<UpdateRecipeModel> updateRecipeModelValidator;
    private readonly IModelValidator<AddCommentModel> addCommentModelValidator;
    private readonly IModelValidator<UpdateCommentModel> updateCommentModelValidator;
    private readonly IModelValidator<AddCompositionFieldModel> addCompositionFieldModelValidator;
    private readonly IModelValidator<UpdateCompositionFieldModel> updateCompositionFieldModelValidator;

    public RecipeService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IRabbitMqTask rabbitMqTask,
        IModelValidator<AddRecipeModel> addRecipeModelValidator,
        IModelValidator<UpdateRecipeModel> updateRecipeModelValidator,
        IModelValidator<AddCommentModel> addCommentModelValidator,
        IModelValidator<UpdateCommentModel> updateCommentModelValidator,
        IModelValidator<AddCompositionFieldModel> addCompositionFieldModelValidator,
        IModelValidator<UpdateCompositionFieldModel> updateCompositionFieldModelValidator)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.rabbitMqTask = rabbitMqTask;
        this.addRecipeModelValidator = addRecipeModelValidator;
        this.updateRecipeModelValidator = updateRecipeModelValidator;
        this.addCommentModelValidator = addCommentModelValidator;
        this.updateCommentModelValidator = updateCommentModelValidator;
        this.addCompositionFieldModelValidator = addCompositionFieldModelValidator;
        this.updateCompositionFieldModelValidator = updateCompositionFieldModelValidator;
    }

    //-------------------------------------------------------------------------------------------------------------

    #region --------------------------------------------------Recipes--------------------------------------------------

    public async Task<IEnumerable<RecipeModel>> GetRecipes(string recipeName = "", int categoryId = 0, string authorNickname = "", int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipes = context
            .Recipes
            .Include(x => x.CompositionFields).ThenInclude(x => x.Ingredient)   //подтягиваю данные из связанной таблицы для последующего мапинга
            .Include(x => x.Category)
            .Include(x => x.Author)
            .AsQueryable();

        if (categoryId > 0)
            recipes = recipes.Where(x => x.CategoryId.Equals(categoryId));


        if (recipeName != "")
            recipes = recipes.Where(x => x.Title.Contains(recipeName));

        if (authorNickname != "")
            recipes = recipes.Where(x => x.Author.UserName.Equals(authorNickname));

        recipes = recipes
            .OrderByDescending(o => o.Id)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Min(limit, 100));

        var data = (await recipes.ToListAsync()).Select(recipe => mapper.Map<RecipeModel>(recipe)).ToList();

        return data;
    }

    public async Task<RecipeModel> GetRecipe(int recipeId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes
            .Include(x => x.CompositionFields).ThenInclude(x => x.Ingredient)   //подтягиваю данные из связанной таблицы для последующего мапинга
            .Include(x => x.Category)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id.Equals(recipeId))
            ?? throw new ProcessException($"The recipe (id: {recipeId}) was not found");

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

        await rabbitMqTask.MailingNewRecipe(recipe.Id);     //кидаю задачу на рассылку о новом рецепте

        var response = mapper.Map<RecipeModel>(await GetRecipe(recipe.Id));

        return response;
    }

    public async Task<RecipeModel> UpdateRecipe(UpdateRecipeModel model)
    {
        updateRecipeModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(model.RecipeId))
            ?? throw new ProcessException($"The recipe (id: {model.RecipeId}) was not found");

        if(recipe.AuthorId != model.RequestAuthor)
            throw new ProcessException($"You can't change someone else's recipe");

        recipe = mapper.Map(model, recipe);

        context.Recipes.Update(recipe);
        context.SaveChanges();

        var response = mapper.Map<RecipeModel>(await GetRecipe(model.RecipeId));

        return response;
    }

    public async Task DeleteRecipe(int recipeId, Guid requestAuthor)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(recipeId))
            ?? throw new ProcessException($"The recipe (id: {recipeId}) was not found");

        if (recipe.AuthorId != requestAuthor)
            throw new ProcessException($"You can't delete someone else's recipe");

        context.Remove(recipe);
        context.SaveChanges();
    }

    #endregion


    #region -------------------------------------------------Comments--------------------------------------------------

    public async Task<IEnumerable<CommentModel>> GetComments(int recipeId, int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(recipeId))
            ?? throw new ProcessException($"The recipe (id: {recipeId}) was not found");

        var comments = context
            .Comments
            .Include(x => x.Author)
            .AsQueryable();

        comments = comments
            .Where(s => s.RecipeId.Equals(recipeId))
            .OrderByDescending(o => o.Id)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Min(limit, 100));

        var data = (await comments.ToListAsync()).Select(comment => mapper.Map<CommentModel>(comment)).ToList();

        return data;
    }

    public async Task<CommentModel> AddComment(AddCommentModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        model.RecipeId = model.RecipeId;
        addCommentModelValidator.Check(model);

        var comment = mapper.Map<Comment>(model);

        await context.Comments.AddAsync(comment);
        context.SaveChanges();

        await rabbitMqTask.MailingNewComment(comment.Id); //кидаю задачу на рассылку о новом комментарии к рецепту

        var response = mapper.Map<CommentModel>(await context.Comments.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(comment.Id)));

        return response;
    }

    public async Task<CommentModel> UpdateComment(UpdateCommentModel model)
    {
        updateCommentModelValidator.Check(model);


        using var context = await contextFactory.CreateDbContextAsync();

        var comment = await context.Comments.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(model.CommentId))
            ?? throw new ProcessException($"The comment (id: {model.CommentId}) was not found");

        if (comment.AuthorId != model.RequestAuthor)
            throw new ProcessException($"You can't change someone else's comment");

        comment = mapper.Map(model, comment);

        context.Comments.Update(comment);
        context.SaveChanges();

        var response = mapper.Map<CommentModel>(await context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(model.CommentId)));

        return response;
    }

    public async Task DeleteComment(int commentId, Guid requestAuthor)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(commentId))
            ?? throw new ProcessException($"The comment (id: {commentId}) was not found");

        if (comment.AuthorId != requestAuthor)
            throw new ProcessException($"You can't delete someone else's comment");


        context.Remove(comment);
        context.SaveChanges();
    }
    #endregion


    #region ---------------------------------------------CompositionFields---------------------------------------------
    public async Task<CompositionFieldModel> AddCompositionField(AddCompositionFieldModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(model.RecipeId));
        if (recipe.AuthorId != model.RequestAuthor)
            throw new ProcessException($"You can't add an ingredient to someone else's recipe");

        addCompositionFieldModelValidator.Check(model);

        var compositionField = mapper.Map<CompositionField>(model);

        await context.CompositionFields.AddAsync(compositionField);
        context.SaveChanges();

        var response = mapper.Map<CompositionFieldModel>(await context.CompositionFields.Include(i => i.Ingredient).FirstOrDefaultAsync(x => x.Id.Equals(compositionField.Id)));

        return response;
    }

    public async Task<CompositionFieldModel> UpdateCompositionField(UpdateCompositionFieldModel model)
    {
        updateCompositionFieldModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var compositionField = await context.CompositionFields.FirstOrDefaultAsync(x => x.Id.Equals(model.CompositionFieldId))
            ?? throw new ProcessException($"The compositionField (id: {model.CompositionFieldId}) was not found");

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(compositionField.RecipeId));
        if (recipe.AuthorId != model.RequestAuthor)
            throw new ProcessException($"You can't change an ingredient in someone else's recipe");

        compositionField = mapper.Map(model, compositionField);

        context.CompositionFields.Update(compositionField);
        context.SaveChanges();

        var response = mapper.Map<CompositionFieldModel>(await context.CompositionFields.Include(i => i.Ingredient).FirstOrDefaultAsync(x => x.Id.Equals(model.CompositionFieldId)));

        return response;
    }

    public async Task DeleteCompositionField(int compositionFieldId, Guid requestAuthor)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var compositionField = await context.CompositionFields.FirstOrDefaultAsync(x => x.Id.Equals(compositionFieldId))
            ?? throw new ProcessException($"The compositionField (id: {compositionFieldId}) was not found");

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(compositionField.RecipeId));
        if (recipe.AuthorId != requestAuthor)
            throw new ProcessException($"You can't delete an ingredient in someone else's recipe");

        context.Remove(compositionField);
        context.SaveChanges();
    }
    #endregion


    #region -------------------------------------------------Addition--------------------------------------------------
    public async Task<IEnumerable<CategoryModel>> GetCategories()
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var categories = context.Categories;

        var data = (await categories.ToListAsync()).Select(category => mapper.Map<CategoryModel>(category)).ToList();

        return data;
    }

    public async Task<IEnumerable<IngredientModel>> GetIngredients()
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var ingredients = context
            .Ingredients
            .OrderBy(o => o.Name);

        var data = (await ingredients.ToListAsync()).Select(ingredient => mapper.Map<IngredientModel>(ingredient)).ToList();

        return data;
    }
    #endregion
}
