
namespace AuthService.Api.Services.Cookies;

public class TokenCookieService : ITokenCookieService
{
    public void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken)
    {
        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };

        var refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
        response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);
    }

    public void ClearAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete("accessToken");
        response.Cookies.Delete("refreshToken");
    }
}
