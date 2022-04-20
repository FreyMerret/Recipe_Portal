using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class AddRecipeCompositionFieldRequest
{
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class AddRecipeRequest
{
    public int CategoryId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public virtual List<AddRecipeCompositionFieldRequest> RecipeCompositionFields { get; set; }
}

public class AddRecipeRequestValidator : AbstractValidator<AddRecipeRequest>
{
    public AddRecipeRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");        
    }
}

public class AddRecipeRequestProfile : Profile
{
    public AddRecipeRequestProfile()
    {
        CreateMap<AddRecipeRequest, AddRecipeModel>()
            .ForMember(d => d.RecipeCompositionFields, a => a.MapFrom(s => s.RecipeCompositionFields)); // from s to d

        CreateMap<AddRecipeCompositionFieldRequest, AddRecipeComposititonFieldModel>();
    }
}
