using AutoMapper;
using RecipePortal.UserAccountService.Models;

namespace RecipePortal.API.Controllers.UserAccounts.Models;

public class UserAccountResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}

public class UserAccountResponseProfile : Profile
{
    public UserAccountResponseProfile()
    {
        CreateMap<UserAccountModel, UserAccountResponse>();
    }
}
