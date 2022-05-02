namespace RecipePortal.Web;

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient,
                       AuthenticationStateProvider authenticationStateProvider,
                       ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var url = $"{Settings.IdentityRoot}/connect/token";

        var request_body = new[] 
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", Settings.ClientId),
            new KeyValuePair<string, string>("client_secret", Settings.ClientSecret),
            new KeyValuePair<string, string>("username", loginModel.Username!),
            new KeyValuePair<string, string>("password", loginModel.Password!)
        };

        var requestContent = new FormUrlEncodedContent(request_body);

        var response = await _httpClient.PostAsync(url, requestContent);

        var content = await response.Content.ReadAsStringAsync();

        var loginResult = JsonSerializer.Deserialize<LoginResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        if (!response.IsSuccessStatusCode)
        {
            return loginResult;
        }

        await _localStorage.SetItemAsync("authToken", loginResult.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", loginResult.RefreshToken);
        await _localStorage.SetItemAsStringAsync("username", loginModel.Username);

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Username!);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        await _localStorage.RemoveItemAsync("username");

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string> GetCurrentUsername()
    {
        return await _localStorage.GetItemAsStringAsync("username");
    }

    public async Task RefreshJWT()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("refreshToken");

        if (string.IsNullOrWhiteSpace(savedToken))  //если проблемы с рефреш токеном, то выбрасываем на страницу авторизации
            Logout();

        //если он есть, то идем обновляться
        var url = $"{Settings.IdentityRoot}/connect/token";

        var refresh_token = await _localStorage.GetItemAsync<string>("refreshToken");

        var request_body = new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", Settings.ClientId),
            new KeyValuePair<string, string>("client_secret", Settings.ClientSecret),
            new KeyValuePair<string, string>("refresh_token", refresh_token)
        };

        var requestContent = new FormUrlEncodedContent(request_body);

        var response = await _httpClient.PostAsync(url, requestContent);

        var content = await response.Content.ReadAsStringAsync();

        var loginResult = JsonSerializer.Deserialize<LoginResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        if (!response.IsSuccessStatusCode)  //ошибка при обновлении токена -> выбрасываем на страницу авторизации
            Logout();

        await _localStorage.SetItemAsync("authToken", loginResult.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", loginResult.RefreshToken);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

    }
}
