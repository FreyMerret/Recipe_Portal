using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class UpdateCompositionFieldModel
{
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class UpdateCompositionFieldModelValidator : AbstractValidator<UpdateCompositionFieldModel>
{
    public UpdateCompositionFieldModelValidator()
    {
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .MaximumLength(50).WithMessage("Too long quantity");
    }
}

public class UpdateCompositionFeildModelProfile : Profile
{
    public UpdateCompositionFeildModelProfile()
    {
        CreateMap<UpdateCompositionFieldModel, CompositionField>();
    }
}
