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

    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet("")]
    public async Task<IEnumerable<RecipeResponse>> GetRecipes()
    {
        var recipes = await recipeService.GetRecipes();
        var response = mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet("{id}")]
    public async Task<RecipeResponse> GetRecipeById([FromRoute] int id)
    {
        var recipe = await recipeService.GetRecipe(id);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPost("")]
    public async Task<RecipeResponse> AddRecipe([FromBody] AddRecipeRequest request)
    {
        var model = mapper.Map<AddRecipeModel>(request);
        //model.Author = "FreyMerret";
        //model.Author = HttpContext.User.Identity.Name;        //допилить когда будет фронт
        var recipe = await recipeService.AddRecipe(model);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe([FromRoute] int id, [FromBody] UpdateRecipeRequest request)
    {
        var model = mapper.Map<UpdateRecipeModel>(request);
        await recipeService.UpdateRecipe(id, model);

        return Ok();
    }

    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe ([FromRoute] int id)
    {
        await recipeService.DeleteRecipe(id);

        return Ok();
    }
}
