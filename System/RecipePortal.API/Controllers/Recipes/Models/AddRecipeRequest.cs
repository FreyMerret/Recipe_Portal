using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class AddRecipeRequest
{
    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public string Ingredients { get; set; } //выбирается в checkbox и формируется строка на фронте

    public int CategoryId { get; set; }     //выбирается в radiobutton на фронте
}

public class AddRecipeResponseValidator : AbstractValidator<AddRecipeRequest>
{
    public AddRecipeResponseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(x => x.ShortDescription).MaximumLength(200).WithMessage("Short description is too long");
    }
}

public class AddRecipeRequestProfile : Profile
{
    public AddRecipeRequestProfile()
    {
        CreateMap<AddRecipeRequest, AddRecipeModel>()
            .ForMember(d => d.Title, a => a.MapFrom(s => s.Title)); // from s to d
    }
}
