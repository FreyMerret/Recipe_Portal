namespace RecipePortal.Web;

using System.Threading.Tasks;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task RefreshJWT();
    Task Logout();
    Task<string> GetCurrentUsername();
}
