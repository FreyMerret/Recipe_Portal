using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipePortal.API.Controllers.Recipes.Models;
using RecipePortal.RecipeService;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes;

[Route("api/v{version:apiVersion}/resipes")]
[ApiController]
[ApiVersion("1.0")]
public class ResipesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<ResipesController> logger;
    private readonly IRecipeService recipeService;

    public ResipesController(IMapper mapper, ILogger<ResipesController> logger, IRecipeService recipeService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.recipeService = recipeService;
    }

    [HttpGet("")]
    public async Task<IEnumerable<RecipeResponse>> GetRecipes()
    {
        var recipes = await recipeService.GetRecipes();
        var response = mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    [HttpGet("{id}")]
    public async Task<RecipeResponse> GetRecipeById([FromRoute] int id)
    {
        var recipe = await recipeService.GetRecipe(id);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [HttpPost("")]
    public async Task<RecipeResponse> AddRecipe([FromBody] AddRecipeRequest request)
    {
        var model = mapper.Map<AddRecipeModel>(request);
        var recipe = await recipeService.AddRecipe(model);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe([FromRoute] int id, [FromBody] UpdateRecipeRequest request)
    {
        var model = mapper.Map<UpdateRecipeModel>(request);
        await recipeService.UpdateRecipe(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe ([FromRoute] int id)
    {
        await recipeService.DeleteRecipe(id);

        return Ok();
    }
}
