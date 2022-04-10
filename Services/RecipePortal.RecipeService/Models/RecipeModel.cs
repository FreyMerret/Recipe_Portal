using AutoMapper;
using RecipePortal.Db.Entities;
using static RecipePortal.RecipeService.Models.RecipeModel;

namespace RecipePortal.RecipeService.Models;

public class RecipeModel
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

    public virtual List<CompositionField> Composition { get; set; } //список ингридиентов и их количество(string)

    public Guid AuthorId { get; set; }

    public string Author { get; set; }

    public int CategoryId { get; set; }

    public string Category { get; set; }
}

public class RecipeModelProfile : Profile
{
    public RecipeModelProfile()
    {
        CreateMap<Recipe, RecipeModel>()
            .ForMember(d => d.Category, a => a.MapFrom(src => src.Category.Title))
            .ForMember(d => d.Author, a => a.MapFrom(src => src.Author.Name))
            .ForMember(d => d.Composition,a => a.MapFrom(src => src.Composition));
    }
}