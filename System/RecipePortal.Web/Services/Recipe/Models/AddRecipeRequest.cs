using FluentValidation;

namespace RecipePortal.Web;

public class AddRecipeCompositionFieldItem
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

    public virtual List<AddRecipeCompositionFieldItem> RecipeCompositionFields { get; set; }
}
public class AddRecipeRequestValidator : AbstractValidator<AddRecipeRequest>
{
    public AddRecipeRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(v => v.CategoryId)
            .GreaterThan(0).WithMessage("Please, select an category");

        RuleFor(v => v.Description)
             .MaximumLength(1024).WithMessage("Description length must be less then 1024");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<AddRecipeRequest>.CreateWithOptions((AddRecipeRequest)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}

public class AddRecipeCompositionFieldItemValidator : AbstractValidator<AddRecipeCompositionFieldItem>
{
    public AddRecipeCompositionFieldItemValidator()
    {
        RuleFor(v => v.IngredientId)
            .GreaterThan(0).WithMessage("Please, select an ingredient");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .MaximumLength(50).WithMessage("Too long quantity");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<AddRecipeCompositionFieldItem>.CreateWithOptions((AddRecipeCompositionFieldItem)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}