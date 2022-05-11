namespace RecipePortal.Web;

public interface IHttpClientBugCatcher
{
    Task<string> GetAsync(string url);
    Task<string> PostAsync(string url, StringContent request);
    Task<string> PutAsync(string url, StringContent request);
    Task<string> DeleteAsync(string url);
}
