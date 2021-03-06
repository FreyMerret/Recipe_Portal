using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class ShortRecipeModel
{
    public int RecipeId { get; set; }

    public string Author { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public virtual List<CompositionFieldModel> CompositionFields { get; set; }
}

public class ShortRecipeModelProfile : Profile
{
    public ShortRecipeModelProfile()
    {
        CreateMap<Recipe, ShortRecipeModel>()
            .ForMember(d => d.RecipeId, a => a.MapFrom(src => src.Id))
            .ForMember(d => d.Category, a => a.MapFrom(src => src.Category.Title))
            .ForMember(d => d.Author, a => a.MapFrom(src => src.Author.UserName))
            .ForMember(d => d.CompositionFields, a => a.MapFrom(src => src.CompositionFields));
    }
}