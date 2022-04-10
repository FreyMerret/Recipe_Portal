using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class RecipeCompositionFieldResponse
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public string Quantity { get; set; }
}

public class RecipeResponse
{
    public int Id { get; set; }

    public Guid AuthorId { get; set; }
    public string Author { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public virtual List<RecipeCompositionFieldResponse> RecipeCompositionFields { get; set; }
}
public class RecipeResponseProfile : Profile
{
    public RecipeResponseProfile()
    {
        CreateMap<RecipeModel, RecipeResponse>();

        CreateMap<RecipeCompositionFieldModel, RecipeCompositionFieldResponse>();
    }
}

