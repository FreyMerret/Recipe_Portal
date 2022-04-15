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
    private readonly IModelValidator<AddCommentModel> addCommentModelValidator;
    private readonly IModelValidator<UpdateCommentModel> updateCommentModelValidator;
    private readonly IModelValidator<AddCompositionFieldModel> addCompositionFieldModelValidator;
    private readonly IModelValidator<UpdateCompositionFieldModel> updateCompositionFieldModelValidator;

    public RecipeService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<AddRecipeModel> addRecipeModelValidator,
        IModelValidator<UpdateRecipeModel> updateRecipeModelValidator,
        IModelValidator<AddCommentModel> addCommentModelValidator,
        IModelValidator<UpdateCommentModel> updateCommentModelValidator,
        IModelValidator<AddCompositionFieldModel> addCompositionFieldModelValidator,
        IModelValidator<UpdateCompositionFieldModel> updateCompositionFieldModelValidator)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.addRecipeModelValidator = addRecipeModelValidator;
        this.updateRecipeModelValidator = updateRecipeModelValidator;
        this.addCommentModelValidator = addCommentModelValidator;
        this.updateCommentModelValidator = updateCommentModelValidator;
        this.addCompositionFieldModelValidator = addCompositionFieldModelValidator;
        this.updateCompositionFieldModelValidator = updateCompositionFieldModelValidator;
    }

    //-------------------------------------------------------------------------------------------------------------

    #region Recipes

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


        var response = mapper.Map<RecipeModel>(await GetRecipe(recipe.Id));

        return response;
    }

    public async Task<RecipeModel> UpdateRecipe(int recipeId, UpdateRecipeModel model)
    {
        updateRecipeModelValidator.Check(model);


        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(recipeId))
            ?? throw new ProcessException($"The recipe (id: {recipeId}) was not found");

        recipe = mapper.Map(model, recipe);

        context.Recipes.Update(recipe);
        context.SaveChanges();

        var response = mapper.Map<RecipeModel>(await GetRecipe(recipeId));

        return response;
    }

    public async Task DeleteRecipe(int recipeId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(recipeId))
            ?? throw new ProcessException($"The recipe (id: {recipeId}) was not found");

        context.Remove(recipe);
        context.SaveChanges();
    }

    #endregion


    #region Comments

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
            .Skip(Math.Max(offset, 0))
            .Take(Math.Min(limit, 100));

        var data = (await comments.ToListAsync()).Select(comment => mapper.Map<CommentModel>(comment)).ToList();

        return data;
    }

    public async Task<CommentModel> AddComment(int recipeId, AddCommentModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        model.RecipeId = recipeId;
        addCommentModelValidator.Check(model);

        var comment = mapper.Map<Comment>(model);

        await context.Comments.AddAsync(comment);
        context.SaveChanges();

        var response = mapper.Map<CommentModel>(await context.Comments.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(comment.Id)));

        return response;
    }

    public async Task<CommentModel> UpdateComment(int commentId, UpdateCommentModel model)
    {
        updateCommentModelValidator.Check(model);


        using var context = await contextFactory.CreateDbContextAsync();

        var comment = await context.Comments.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(commentId))
            ?? throw new ProcessException($"The comment (id: {commentId}) was not found");

        comment = mapper.Map(model, comment);

        context.Comments.Update(comment);
        context.SaveChanges();

        var response = mapper.Map<CommentModel>(await context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(commentId)));

        return response;
    }

    public async Task DeleteComment(int commentId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(commentId))
            ?? throw new ProcessException($"The comment (id: {commentId}) was not found");

        context.Remove(comment);
        context.SaveChanges();
    }
    #endregion

    #region CompositionFields
    public async Task<CompositionFieldModel> AddCompositionField(int recipeId, AddCompositionFieldModel model)
    {
        model.RecipeId = recipeId;

        using var context = await contextFactory.CreateDbContextAsync();
        addCompositionFieldModelValidator.Check(model);

        var compositionField = mapper.Map<CompositionField>(model);

        await context.CompositionFields.AddAsync(compositionField);
        context.SaveChanges();

        var response = mapper.Map<CompositionFieldModel>(await context.CompositionFields.FirstOrDefaultAsync(x => x.Id.Equals(compositionField.Id)));

        return response;
    }

    public async Task<CompositionFieldModel> UpdateCompositionField(int compositionFieldId, UpdateCompositionFieldModel model)
    {
        updateCompositionFieldModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var compositionField = await context.CompositionFields.FirstOrDefaultAsync(x => x.Id.Equals(compositionFieldId))
            ?? throw new ProcessException($"The compositionField (id: {compositionFieldId}) was not found");

        compositionField = mapper.Map(model, compositionField);

        context.CompositionFields.Update(compositionField);
        context.SaveChanges();

        var response = mapper.Map<CompositionFieldModel>(await context.CompositionFields.FirstOrDefaultAsync(x => x.Id.Equals(compositionFieldId)));

        return response;
    }

    public async Task DeleteCompositionField(int compositionFieldId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var compositionField = await context.CompositionFields.FirstOrDefaultAsync(x => x.Id.Equals(compositionFieldId))
            ?? throw new ProcessException($"The compositionField (id: {compositionFieldId}) was not found");

        context.Remove(compositionField);
        context.SaveChanges();
    }
    #endregion
}
