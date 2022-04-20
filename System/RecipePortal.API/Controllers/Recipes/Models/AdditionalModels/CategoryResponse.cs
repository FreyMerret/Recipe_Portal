using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class CategoryResponse
{
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
}

public class CategoryResponseProfile : Profile
{
    public CategoryResponseProfile()
    {
        CreateMap<CategoryModel, CategoryResponse>();
    }
}

