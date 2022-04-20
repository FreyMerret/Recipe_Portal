using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class UpdateCommentModel
{
    public Guid RequestAuthor { get; set; }
    public int CommentId { get; set; }
    public string CommentText { get; set; }
}

public class UpdateCommentModelValidator : AbstractValidator<UpdateCommentModel>
{
    public UpdateCommentModelValidator()
    {
        RuleFor(x => x.CommentText)
            .NotEmpty().WithMessage("Comment text is required")
            .MaximumLength(1000).WithMessage("Too long comment");
    }
}

public class UpdateCommentModelProfile : Profile
{
    public UpdateCommentModelProfile()
    {
        CreateMap<UpdateCommentModel, Comment>();
    }
}
