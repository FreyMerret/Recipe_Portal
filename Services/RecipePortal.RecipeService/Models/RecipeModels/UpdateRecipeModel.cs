using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class UpdateRecipeModel
{
    public Guid RequestAuthor { get; set; }

    public int RecipeId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }
}

public class UpdateRecipeModelValidator : AbstractValidator<UpdateRecipeModel>
{
    public UpdateRecipeModelValidator()
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

public class UpdateRecipeModelProfile : Profile
{
    public UpdateRecipeModelProfile()
    {
        CreateMap<UpdateRecipeModel, Recipe>()
            .ForMember(d => d.Title, a => a.MapFrom(s => s.Title)); // from s to d
    }
}