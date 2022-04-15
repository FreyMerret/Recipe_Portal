using AutoMapper;
using FluentValidation;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class AddCompositionFieldRequest
{
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class AddCompositionFieldRequestValidator : AbstractValidator<AddCompositionFieldRequest>
{
    public AddCompositionFieldRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .MaximumLength(50).WithMessage("Too long quantity");
    }
}

public class AddCompositionFeildRequestProfile : Profile
{
    public AddCompositionFeildRequestProfile()
    {
        CreateMap<AddCompositionFieldRequest, AddCompositionFieldModel>()
            .ForMember(d => d.RecipeId, a=>a.Ignore());
    }
}
