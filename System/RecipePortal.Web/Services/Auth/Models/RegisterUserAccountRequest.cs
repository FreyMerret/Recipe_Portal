using FluentValidation;

namespace RecipePortal.Web;

public class RegisterUserAccountRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserAccountRequestValidator : AbstractValidator<RegisterUserAccountRequest>
{
    public RegisterUserAccountRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Nickname is long.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(50).WithMessage("Password is long.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<RegisterUserAccountRequest>.CreateWithOptions((RegisterUserAccountRequest)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
