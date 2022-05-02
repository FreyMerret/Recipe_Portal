using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class IngredientResponse
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
}

public class IngredientResponseProfile : Profile
{
    public IngredientResponseProfile()
    {
        CreateMap<IngredientModel, IngredientResponse>();
    }
}
