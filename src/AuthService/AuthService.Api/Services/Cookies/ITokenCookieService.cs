namespace AuthService.Api.Services.Cookies;

public interface ITokenCookieService
{
    void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken);
    void ClearAuthCookies(HttpResponse response);
}
