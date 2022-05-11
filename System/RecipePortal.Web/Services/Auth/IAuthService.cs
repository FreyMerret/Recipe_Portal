namespace RecipePortal.Web;

using System.Threading.Tasks;

public interface IAuthService
{
    Task<bool> Register(RegisterUserAccountRequest registerModel);
    Task<LoginResult> Login(LoginModel loginModel);
    Task RefreshJWT();
    Task Logout();
    Task<string> GetCurrentUsername();
    Task<bool> EmailConfirmation(string email, string emailConfirmToken);
}
