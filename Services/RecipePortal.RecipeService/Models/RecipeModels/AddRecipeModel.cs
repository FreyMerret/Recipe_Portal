using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class AddRecipeComposititonFieldModel
{
    public int IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class AddRecipeModel
{
    public Guid AuthorId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public List<AddRecipeComposititonFieldModel> RecipeCompositionFields { get; set; }
}

public class AddRecipeModelValidator : AbstractValidator<AddRecipeModel>
{
    public AddRecipeModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(x => x.Description).MaximumLength(200).WithMessage("Too long description");
    }
}

public class AddRecipeModelProfile : Profile
{
    public AddRecipeModelProfile()
    {
        CreateMap<AddRecipeModel, Recipe>()
            .ForMember(d => d.CompositionFields, a => a.MapFrom(s => s.RecipeCompositionFields)); // from s to d

        CreateMap<AddRecipeComposititonFieldModel, CompositionField>();
    }
}
