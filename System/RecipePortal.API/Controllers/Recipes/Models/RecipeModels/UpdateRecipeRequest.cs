using AutoMapper;
using FluentValidation;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class UpdateRecipeRequest
{
    public int CategoryId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }
}

public class UpdateRecipeRequestValidator : AbstractValidator<UpdateRecipeRequest>
{
    public UpdateRecipeRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");
    }
}

public class UpdateRecipeRequestProfile : Profile
{
    public UpdateRecipeRequestProfile()
    {
        CreateMap<UpdateRecipeRequest, UpdateRecipeModel>();
    }
}
