using AutoMapper;
using RecipePortal.RecipeService.Models;

namespace RecipePortal.API.Controllers.Recipes.Models;

public class CommentResponse
{
    public int Id { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorNickname { get; set; }

    public string CommentText { get; set; }
}
public class CommentResponseProfile : Profile
{
    public CommentResponseProfile()
    {
        CreateMap<CommentModel, CommentResponse>();
    }
}

