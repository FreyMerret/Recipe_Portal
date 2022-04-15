using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;
public class AddCompositionFieldModel
{
    public int RecipeId { get; set; }   //добавление ингридиента может быть только в существующем рецепте, поэтому тут мы уже знаем Id рецепта
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class AddCompositionFieldModelValidator : AbstractValidator<AddCompositionFieldModel>
{
    public AddCompositionFieldModelValidator()
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
        CreateMap<AddCompositionFieldModel, CompositionField>();
    }
}
