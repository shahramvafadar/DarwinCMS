using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Auth;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Shared.Security;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

namespace DarwinCMS.WebAdmin.Services.Auth;

/// <summary>
/// Provides login and logout functionality for admin users using cookie-based authentication.
/// </summary>
public class LoginService : ILoginService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;

    /// <summary>
    /// Initializes the login service with required repositories and HTTP context accessor.
    /// </summary>
    /// <param name="httpContextAccessor">Access to the current HTTP context.</param>
    /// <param name="userRepository">Repository for retrieving and updating users.</param>
    /// <param name="userRoleRepository">Repository for retrieving role assignments for users.</param>
    /// <param name="rolePermissionRepository">Repository for retrieving permissions assigned to roles.</param>
    public LoginService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IRolePermissionRepository rolePermissionRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    /// <summary>
    /// Authenticates the user based on email and password, then issues a cookie-based session.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's plain-text password to verify.</param>
    /// <returns>True if login is successful; otherwise false.</returns>
    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return false;

        if (!PasswordHasher.Verify(password, user.PasswordHash))
            return false;

        var roleIds = await _userRoleRepository.GetRoleIdsByUserIdAsync(user.Id);
        var permissions = await _rolePermissionRepository.GetPermissionNamesForRolesAsync(roleIds);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email.Value)
        };

        foreach (var permission in permissions.Distinct())
        {
            claims.Add(new Claim("permission", permission));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);

        return true;
    }

    /// <summary>
    /// Retrieves a user entity by their email address.
    /// </summary>
    /// <param name="email">The email address to look up.</param>
    /// <returns>The matching <see cref="User"/> or null if not found.</returns>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    /// <summary>
    /// Updates the provided user entity in the repository.
    /// </summary>
    /// <param name="user">The user object to update.</param>
    public async Task UpdateUserAsync(User user)
    {
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Logs out the current user by clearing their authentication cookie.
    /// </summary>
    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
