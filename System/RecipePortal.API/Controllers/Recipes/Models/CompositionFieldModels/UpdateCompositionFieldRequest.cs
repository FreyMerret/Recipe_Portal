using AutoMapper;
using FluentValidation;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class UpdateCompositionFieldRequest
{
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class UpdateCompositionFieldRequestValidator : AbstractValidator<UpdateCompositionFieldRequest>
{
    public UpdateCompositionFieldRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .MaximumLength(50).WithMessage("Too long quantity");
    }
}

public class UpdateCompositionFeildRequestProfile : Profile
{
    public UpdateCompositionFeildRequestProfile()
    {
        CreateMap<UpdateCompositionFieldRequest, UpdateCompositionFieldModel>();
    }
}
