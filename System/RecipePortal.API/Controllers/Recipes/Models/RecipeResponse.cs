using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class RecipeResponse
{
    //допилить
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public struct CompositionField
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string Quantity { get; set; }
    }

    public virtual List<CompositionField> Composition { get; set; }

    public Guid AuthorId { get; set; }

    public string Author { get; set; }

    public int CategoryId { get; set; }

    public string Category { get; set; }
}
public class RecipeResponseProfile : Profile
{
    public RecipeResponseProfile()
    {
        CreateMap<RecipeModel, RecipeResponse>();
            //.ForMember(d => d.Category, a => a.MapFrom(src => (src.Category ?? "oshibka2")));
    }
}

