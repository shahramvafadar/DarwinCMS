using DarwinCMS.Application.Services.Auth;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Auth;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Handles admin login, logout, password recovery, and access control.
/// </summary>
[Area("Admin")]
public class AccountController : Controller
{
    private readonly ILoginService _loginService;
    private readonly IPasswordResetService _passwordResetService;

    /// <summary>
    /// Creates a new instance of the AccountController.
    /// </summary>
    /// <param name="loginService">Service used to handle login and logout operations.</param>
    /// <param name="passwordResetService">Service used to generate and validate password reset tokens.</param>
    public AccountController(
        ILoginService loginService,
        IPasswordResetService passwordResetService)
    {
        _loginService = loginService;
        _passwordResetService = passwordResetService;
    }

    /// <summary>
    /// Displays the login form.
    /// </summary>
    /// <param name="returnUrl">The URL to redirect to after login.</param>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };
        return View(model);
    }

    /// <summary>
    /// Processes the login form and performs authentication.
    /// </summary>
    /// <param name="model">Login form data.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var success = await _loginService.LoginAsync(model.Email, model.Password);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Dashboard");
    }

    /// <summary>
    /// Logs the user out and ends the session.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _loginService.LogoutAsync();
        return RedirectToAction("Login");
    }

    /// <summary>
    /// Displays the "access denied" page when the user lacks permissions.
    /// </summary>
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    /// <summary>
    /// Displays the form to initiate a password reset request.
    /// </summary>
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    /// <summary>
    /// Handles forgot password form submissions and sends a reset link.
    /// </summary>
    /// <param name="model">Forgot password form input.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _loginService.GetUserByEmailAsync(model.Email);
        if (user != null && user.IsActive)
        {
            var resetToken = await _passwordResetService.GenerateTokenAsync(model.Email);

            var resetLink = Url.Action("ResetPassword", "Account", new
            {
                area = "Admin",
                token = resetToken.Token,
                email = model.Email
            }, Request.Scheme);

            // TODO: Send email (for now simulate)
            Console.WriteLine($"[DEBUG] Reset Link: {resetLink}");
        }

        TempData["Message"] = "If the email is valid, a reset link will be sent.";
        return RedirectToAction("ForgotPasswordConfirmation");
    }

    /// <summary>
    /// Displays confirmation that password reset email was sent.
    /// </summary>
    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    /// <summary>
    /// Displays the form to reset the user's password using a token.
    /// </summary>
    /// <param name="token">The unique password reset token.</param>
    /// <param name="email">The user's email address.</param>
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPasswordViewModel
        {
            Token = token,
            Email = email
        };

        return View(model);
    }

    /// <summary>
    /// Handles submission of the reset password form and updates the password.
    /// </summary>
    /// <param name="model">Reset password form data.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var token = await _passwordResetService.ValidateTokenAsync(model.Token);
        if (token == null || token.Email.ToLower() != model.Email.Trim().ToLower())
        {
            ModelState.AddModelError(string.Empty, "Invalid or expired token.");
            return View(model);
        }

        var user = await _loginService.GetUserByEmailAsync(model.Email);
        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "User account is not valid.");
            return View(model);
        }

        user.SetPassword(model.NewPassword);
        await _loginService.UpdateUserAsync(user);
        await _passwordResetService.InvalidateTokenAsync(token);

        TempData["Message"] = "Your password has been successfully reset.";
        return RedirectToAction("Login");
    }
}
