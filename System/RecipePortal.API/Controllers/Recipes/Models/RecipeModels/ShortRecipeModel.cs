using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class ShortRecipeResponse
{
    public int RecipeId { get; set; }

    public string Author { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public virtual List<CompositionFieldResponse> CompositionFields { get; set; }
}

public class ShortRecipeResponseProfile : Profile
{
    public ShortRecipeResponseProfile()
    {
        CreateMap<ShortRecipeModel, ShortRecipeResponse>()
            .ForMember(d => d.CompositionFields, a => a.MapFrom(src => src.CompositionFields));
    }
}