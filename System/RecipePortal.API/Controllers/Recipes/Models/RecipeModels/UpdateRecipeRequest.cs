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
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(x => x.Description).MaximumLength(200).WithMessage("Too long description");

        RuleFor(x => x.Text).MaximumLength(2000).WithMessage("Too long text");
    }
}

public class UpdateRecipeRequestProfile : Profile
{
    public UpdateRecipeRequestProfile()
    {
        CreateMap<UpdateRecipeRequest, UpdateRecipeModel>();
    }
}
