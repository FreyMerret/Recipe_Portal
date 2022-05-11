using FluentValidation;

namespace RecipePortal.Web;

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

        RuleFor(v => v.CategoryId)
            .GreaterThan(0).WithMessage("Please, select an category");

        RuleFor(v => v.Description)
             .MaximumLength(1024).WithMessage("Description length must be less then 1024");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UpdateRecipeRequest>.CreateWithOptions((UpdateRecipeRequest)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}