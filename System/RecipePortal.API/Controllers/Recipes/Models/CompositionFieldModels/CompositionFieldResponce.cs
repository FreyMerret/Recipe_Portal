using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class CompositionFieldResponse
{
    public int CompositionFieldId { get; set; }
    public string IngredientId { get; set; }
    public string IngredientName { get; set; }
    public string Quantity { get; set; }
}

public class CompositionFeildResponseProfile : Profile
{
    public CompositionFeildResponseProfile()
    {
        CreateMap<CompositionFieldModel, CompositionFieldResponse>();
    }
}
