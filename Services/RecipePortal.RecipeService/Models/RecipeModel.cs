using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class RecipeModel
{
    //допилить
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
}

public class RecipeModelProfile : Profile
{
    public RecipeModelProfile()
    {
        CreateMap<Recipe, RecipeModel>();
    }
}