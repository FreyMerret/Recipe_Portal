using AutoMapper;
using FluentValidation;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class AddCommentModel
{
    public Guid AuthorId { get; set; }
    public string CommentText { get; set; }
    public int RecipeId { get; set; } = 0;

}

public class AddCommentModelValidator : AbstractValidator<AddCommentModel>
{
    public AddCommentModelValidator()
    {
        RuleFor(x => x.CommentText)
            .NotEmpty().WithMessage("Comment text is required")
            .MaximumLength(1000).WithMessage("Too long comment");
    }
}

public class AddCommentModelProfile : Profile
{
    public AddCommentModelProfile()
    {
        CreateMap<AddCommentModel, Comment>();
    }
}
