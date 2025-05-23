using DarwinCMS.Application.Abstractions.Repositories;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Handles login flow with external providers like Google and Microsoft.
/// </summary>
[Area("Admin")]
[Route("Admin/ExternalAuth")]
public class ExternalAuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalAuthController"/> class.
    /// </summary>
    public ExternalAuthController(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IRolePermissionRepository rolePermissionRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    /// <summary>
    /// Starts OAuth login with Google or Microsoft provider.
    /// </summary>
    [HttpGet("SignIn")]
    public IActionResult SignIn(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(Callback), "ExternalAuth", new { returnUrl });

        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl!,
            Items = { { "scheme", provider } }
        };

        return Challenge(properties, provider);
    }

    /// <summary>
    /// OAuth callback handler for external login.
    /// </summary>
    [HttpGet("Callback")]
    public async Task<IActionResult> Callback(string? returnUrl = null)
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded || result.Principal?.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var email = identity.FindFirst(ClaimTypes.Email)?.Value;
        var name = identity.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction("AccessDenied", "Account");

        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null || !user.IsActive)
            return RedirectToAction("AccessDenied", "Account");

        var roleIds = await _userRoleRepository.GetRoleIdsByUserIdAsync(user.Id);
        var permissions = await _rolePermissionRepository.GetPermissionNamesForRolesAsync(roleIds);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email.Value)
        };

        foreach (var permission in permissions.Distinct())
        {
            claims.Add(new Claim("permission", permission));
        }

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            });

        return Redirect(returnUrl ?? Url.Action("Index", "Dashboard", new { area = "Admin" })!);
    }

    /// <summary>
    /// Clears session and logs the user out.
    /// </summary>
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
