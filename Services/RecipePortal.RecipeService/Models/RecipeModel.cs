using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class RecipeCompositionFieldModel
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public string Quantity { get; set; }
}
public class RecipeModel
{
    public int Id { get; set; }

    public Guid AuthorId { get; set; }
    public string Author { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public virtual List<RecipeCompositionFieldModel> RecipeCompositionFields { get; set; }
}

public class RecipeModelProfile : Profile
{
    public RecipeModelProfile()
    {
        CreateMap<Recipe, RecipeModel>()
            .ForMember(d => d.Category, a => a.MapFrom(src => src.Category.Title))
            .ForMember(d => d.Author, a => a.MapFrom(src => src.Author.Name))
            .ForMember(d => d.RecipeCompositionFields, a => a.MapFrom(src => src.RecipeCompositionFields));

        CreateMap<RecipeCompositionField, RecipeCompositionFieldModel>()
            .ForMember(d => d.IngredientId, a => a.MapFrom(src => src.IngredientId))
            .ForMember(d => d.IngredientName, a => a.MapFrom(src => src.Ingredient.Name))
            .ForMember(d => d.Quantity, a => a.MapFrom(src => src.Quantity));
    }
}