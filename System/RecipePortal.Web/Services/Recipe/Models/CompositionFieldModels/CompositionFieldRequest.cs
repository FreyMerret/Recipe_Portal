using FluentValidation;

namespace RecipePortal.Web;

public class CompositionFieldRequest
{
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class CompositionFieldRequestValidator : AbstractValidator<CompositionFieldRequest>
{
    public CompositionFieldRequestValidator()
    {
        RuleFor(v => v.IngredientId)
            .GreaterThan(0).WithMessage("Please, select an ingredient");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .MaximumLength(50).WithMessage("Too long quantity");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CompositionFieldRequest>.CreateWithOptions((CompositionFieldRequest)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}