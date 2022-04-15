using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class AddCommentRequest
{
    public Guid AuthorId { get; set; }
    public string CommentText { get; set; }
}

public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
{
    public AddCommentRequestValidator()
    {
        RuleFor(x => x.CommentText)
            .NotEmpty().WithMessage("Comment text is required")
            .MaximumLength(1000).WithMessage("Too long comment");        
    }
}

public class AddCommentRequestProfile : Profile
{
    public AddCommentRequestProfile()
    {
        CreateMap<AddCommentRequest, AddCommentModel>()
            .ForMember(d => d.RecipeId, opt => opt.Ignore());
    }
}
