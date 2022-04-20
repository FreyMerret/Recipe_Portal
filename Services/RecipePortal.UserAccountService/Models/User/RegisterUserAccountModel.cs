using FluentValidation;

namespace RecipePortal.UserAccountService.Models;

public class RegisterUserAccountModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserAccountModelValidator : AbstractValidator<RegisterUserAccountModel>
{
    public RegisterUserAccountModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("User name is required.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("User name is required.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("User name is required.")
            .MaximumLength(50).WithMessage("Nickname is long.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(50).WithMessage("Password is long.");
    }
}