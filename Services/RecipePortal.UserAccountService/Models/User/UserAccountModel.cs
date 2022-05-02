using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.UserAccountService.Models;

public class UserAccountModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

}

public class UserAccountModelProfile : Profile
{
    public UserAccountModelProfile()
    {
        CreateMap<User, UserAccountModel>();
    }
}
