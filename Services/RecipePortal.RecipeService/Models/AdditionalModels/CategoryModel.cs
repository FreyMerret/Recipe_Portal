using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class CategoryModel
{
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
}

public class CategoryModelProfile : Profile
{
    public CategoryModelProfile()
    {
        CreateMap<Category,CategoryModel>()
            .ForMember(d => d.CategoryId, a => a.MapFrom(src => src.Id))
            .ForMember(d => d.CategoryName, a=>a.MapFrom(src => src.Title));
    }
}
