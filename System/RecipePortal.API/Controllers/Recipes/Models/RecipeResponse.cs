using AutoMapper;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class RecipeResponse
{
    //допилить
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string LongDescription { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
}
public class RecipeResponseProfile : Profile
{
    public RecipeResponseProfile()
    {
        CreateMap<RecipeModel, RecipeResponse>();
            //.ForMember(d => d.ShortDescription, a => a.MapFrom(s => s.ShortDescription)); // from s to d
    }
}

