using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Auth;

/// <summary>
/// Provides user authentication and session-based sign-in for admin panel.
/// </summary>
public interface ILoginService
{
    /// <summary>
    /// Authenticates user and signs them in if valid.
    /// </summary>
    /// <param name="email">User email address</param>
    /// <param name="password">Plain-text password</param>
    /// <returns>True if login succeeded, false otherwise</returns>
    Task<bool> LoginAsync(string email, string password);

    /// <summary>
    /// Signs the current user out of the admin session.
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
    /// specified email address,  or <see langword="null"/> if no user is found.</returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Updates the specified user's information asynchronously.
    /// </summary>
    /// <remarks>This method updates the user's information in the underlying data store.  Ensure that the
    /// <paramref name="user"/> object contains valid and complete data  before calling this method. The operation is
    /// performed asynchronously and does not block the calling thread.</remarks>
    /// <param name="user">The user object containing updated information. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateUserAsync(User user);

}
