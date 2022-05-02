using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class IngredientModel
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
}

public class IngredientResponseProfile : Profile
{
    public IngredientResponseProfile()
    {
        CreateMap<Ingredient, IngredientModel>()
            .ForMember(d => d.IngredientId, a => a.MapFrom(src => src.Id))
            .ForMember(d => d.IngredientName, a => a.MapFrom(src => src.Name));
    }
}
