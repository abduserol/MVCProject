using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PatientsProject.Core.App.Services.Authentication.MVC;

public class CookieAuthService : ICookieAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignIn(int userId, string userName, string[] userRoleNames, DateTime? expiration = default, bool isPersistent = true)
    {
        var claims = new List<Claim>()
        {
            new Claim("Id", userId.ToString()),
            new Claim(ClaimTypes.Name, userName)
        };
        
        foreach (var userRoleName in userRoleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRoleName));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authenticationProperties = new AuthenticationProperties
        {
            // Her zaman session cookie - tarayıcı kapanınca oturum biter
            IsPersistent = false
        };

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);
    }

    public async Task SignOut()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
