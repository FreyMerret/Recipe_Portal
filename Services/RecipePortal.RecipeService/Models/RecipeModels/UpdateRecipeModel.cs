using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class UpdateRecipeModel
{
    public int CategoryId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }
}

public class UpdateRecipeModelValidator : AbstractValidator<UpdateRecipeModel>
{
    public UpdateRecipeModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(x => x.Description).MaximumLength(200).WithMessage("Too long description");
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