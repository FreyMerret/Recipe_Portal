using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

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

    //поля состава рецепта
    public virtual List<CompositionFieldResponse> CompositionFields { get; set; }
}
public class RecipeResponseProfile : Profile
{
    public RecipeResponseProfile()
    {
        CreateMap<RecipeModel, RecipeResponse>();
    }
}

