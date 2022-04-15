using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class UpdateCommentRequest
{
    public string CommentText { get; set; }
}

public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
{
    public UpdateCommentRequestValidator()
    {
        RuleFor(x => x.CommentText)
            .NotEmpty().WithMessage("Comment text is required")
            .MaximumLength(1000).WithMessage("Too long comment");        
    }
}

public class UpdateCommentRequestProfile : Profile
{
    public UpdateCommentRequestProfile()
    {
        CreateMap<UpdateCommentRequest, UpdateCommentModel>();
    }
}
