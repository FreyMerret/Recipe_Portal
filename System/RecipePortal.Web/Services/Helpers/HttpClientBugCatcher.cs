namespace RecipePortal.Web;

public class HttpClientBugCatcher : IHttpClientBugCatcher
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public HttpClientBugCatcher(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<string> GetAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);                     //пробуем получить ответ
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)  //если поймали ошибку, что не авторизованы (или просто истек токен)
        {
            await _authService.RefreshJWT();                                //пробуем обновить токен. В случае неудачи попадем на страницу авторизации
            response = await _httpClient.GetAsync(url);                     //если все хорошо, то повторяем наш запрос
        }

        var content = await response.Content.ReadAsStringAsync();           //считываем ответ
        if (!response.IsSuccessStatusCode)                                  //если пришла еще какая-то ошибка, то возвращаем ее
                throw new Exception(content);

        return content;
    }

    public async Task<string> PostAsync(string url, StringContent request)
    {
        var response = await _httpClient.PostAsync(url, request);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshJWT();
            response = await _httpClient.PostAsync(url, request);
        }

        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
        return content;
    }    

    public async Task<string> PutAsync(string url, StringContent request)
    {
        var response = await _httpClient.PutAsync(url, request);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshJWT();
            response = await _httpClient.PutAsync(url, request);
        }

        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
        return content;
    }
    public async Task<string> DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshJWT();
            response = await _httpClient.DeleteAsync(url);
        }

        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
        return content;
    }
}
