using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipePortal.API.Controllers.Recipes.Models;
using RecipePortal.Common.Security;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes;

[Route("api/v{version:apiVersion}/resipes")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class ResipesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<ResipesController> logger;
    private readonly IRecipeService recipeService;
    private readonly UserManager<User> userManager;

    public ResipesController(IMapper mapper, ILogger<ResipesController> logger, IRecipeService recipeService, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.recipeService = recipeService;
        this.userManager = userManager; //для работы с пользователями
    }

    /// <summary>
    /// Констроллеры, связанные с рецептами
    /// </summary>
    #region Recipes
    
    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet]
    public async Task<IEnumerable<RecipeResponse>> GetRecipes(
        [FromQuery] string recipeName = "",
        [FromQuery] int categoryId = 0,
        [FromQuery] string authorNickname = "",
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var recipes = await recipeService.GetRecipes(recipeName, categoryId, authorNickname, offset, limit);
        var response = mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet("{recipeId}")]
    public async Task<RecipeResponse> GetRecipeById([FromRoute] int recipeId)
    {
        var recipe = await recipeService.GetRecipe(recipeId);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPost]
    public async Task<RecipeResponse> AddRecipe([FromBody] AddRecipeRequest request)
    {
        var model = mapper.Map<AddRecipeModel>(request);
        //model.Author = HttpContext.User.Identity.Name;        //допилить когда будет фронт
        var recipe = await recipeService.AddRecipe(model);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPut("{recipeId}")]
    public async Task<RecipeResponse> UpdateRecipe([FromRoute] int recipeId, [FromBody] UpdateRecipeRequest request)
    {
        var model = mapper.Map<UpdateRecipeModel>(request);
        var recipe = await recipeService.UpdateRecipe(recipeId, model);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> DeleteRecipe([FromRoute] int recipeId)
    {
        await recipeService.DeleteRecipe(recipeId);

        return Ok();
    }

    #endregion

    /// <summary>
    /// Констроллеры, связанные с комментариями
    /// </summary>
    #region Comments

    [RequiredScope(AppScopes.CommentsRead)]
    [HttpGet("{recipeId}/comments")]
    public async Task<IEnumerable<CommentResponse>> GetComments([FromRoute] int recipeId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var comments = await recipeService.GetComments(recipeId, offset, limit);
        var response = mapper.Map<IEnumerable<CommentResponse>>(comments);

        return response;
    }

    [RequiredScope(AppScopes.CommentsWrite)]
    [HttpPost("{recipeId}/comments")]
    public async Task<CommentResponse> AddComment([FromRoute] int recipeId, [FromBody] AddCommentRequest request)
    {
        var model = mapper.Map<AddCommentModel>(request);
        var comment = await recipeService.AddComment(recipeId, model);
        var response = mapper.Map<CommentResponse>(comment);

        return response;
    }

    [RequiredScope(AppScopes.CommentsWrite)]
    [HttpPut("{recipeId}/comments/{commentId}")]
    public async Task<CommentResponse> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentRequest request)
    {
        var model = mapper.Map<UpdateCommentModel>(request);
        var comment = await recipeService.UpdateComment(commentId, model);
        var response = mapper.Map<CommentResponse>(comment);

        return response;
    }

    [RequiredScope(AppScopes.CommentsWrite)]
    [HttpDelete("{recipeId}/comments/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        await recipeService.DeleteComment(commentId);

        return Ok();
    }
    #endregion

    /// <summary>
    /// Констроллеры, связанные с полями рецепта
    /// </summary>
    #region CompositionFields

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPost("{recipeId}/ingredients")]
    public async Task<CompositionFieldResponse> AddCompositionField([FromRoute]int recipeId, [FromBody] AddCompositionFieldRequest request)
    {
        var model = mapper.Map<AddCompositionFieldModel>(request);
        var compositionField = await recipeService.AddCompositionField(recipeId, model);
        var response = mapper.Map<CompositionFieldResponse>(compositionField);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPut("{recipeId}/ingredients/{compositionFieldId}")]
    public async Task<CompositionFieldResponse> UpdateCompositionField([FromRoute] int compositionFieldId, [FromBody] UpdateCompositionFieldRequest request)
    {
        var model = mapper.Map<UpdateCompositionFieldModel>(request);
        var compositionField = await recipeService.UpdateCompositionField(compositionFieldId, model);
        var response = mapper.Map<CompositionFieldResponse>(compositionField);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpDelete("{recipeId}/ingredients/{compositionFieldId}")]
    public async Task<IActionResult> DeleteCompositionField([FromRoute] int compositionFieldId)
    {
        await recipeService.DeleteCompositionField(compositionFieldId);

        return Ok();
    }
    #endregion
}
