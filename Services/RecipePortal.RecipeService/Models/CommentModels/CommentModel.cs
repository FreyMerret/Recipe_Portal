using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class CommentModel
{
    public int Id { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorNickname { get; set; }

    public string CommentText { get; set; }
}

public class CommentModelProfile : Profile
{
    public CommentModelProfile()
    {
        CreateMap<Comment, CommentModel>()
            .ForMember(d => d.AuthorNickname, a => a.MapFrom(src => src.Author.UserName));
    }
}