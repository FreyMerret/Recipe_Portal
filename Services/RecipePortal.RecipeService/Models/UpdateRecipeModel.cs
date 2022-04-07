﻿using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;
public class UpdateRecipeModel
{
    public string Title { get; set; }

    public string ShortDescription { get; set; }

    //public string LongDescription { get; set; }

    //public virtual ICollection<Ingredient> Ingredients { get; set; }

    //public int? AuthorId { get; set; }

    //public virtual User Author { get; set; }

    //public int CategoryId { get; set; }

    //public virtual Category Category { get; set; }

    //public double Rating { get; set; }
    //public string Photos { get; set; }
}

public class UpdateRecipeModelValidator : AbstractValidator<UpdateRecipeModel>
{
    public UpdateRecipeModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Too long title");

        RuleFor(x => x.ShortDescription).MaximumLength(200).WithMessage("Too long description");
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