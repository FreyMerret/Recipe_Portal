using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class AddRecipeModel
{
    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public List<Ingredient> Ingredients { get; set; }

    public Guid AuthorId { get; set; }

    public int CategoryId { get; set; }
}

public class AddRecipeModelValidator : AbstractValidator<AddRecipeModel>
{
    public AddRecipeModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(x => x.ShortDescription).MaximumLength(200).WithMessage("Too long description");
    }
}

public class AddRecipeModelProfile : Profile
{
    public AddRecipeModelProfile()
    {
        CreateMap<AddRecipeModel, Recipe>()
            .ForMember(d => d.Title, a => a.MapFrom(s => s.Title)); // from s to d
    }
}
