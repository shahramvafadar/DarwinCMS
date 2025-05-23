using DarwinCMS.Application.Services.Common;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace DarwinCMS.Infrastructure.Services.Common;

/// <summary>
/// Sends emails using the standard SMTP protocol (e.g., Gmail, Outlook, hosting provider).
/// </summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes the SMTP sender with configuration.
    /// </summary>
    /// <param name="config">Application configuration object used to read SMTP settings.</param>
    public SmtpEmailSender(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Sends an email using SMTP credentials and host settings.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The HTML body of the email.</param>
    public async Task SendAsync(string toEmail, string subject, string body)
    {
        var host = _config["Email:Smtp:Host"];
        var port = int.Parse(_config["Email:Smtp:Port"] ?? "587");
        var username = _config["Email:Smtp:Username"];
        var password = _config["Email:Smtp:Password"];
        var from = _config["Email:Smtp:From"];

        if (string.IsNullOrWhiteSpace(from))
        {
            from = username;
        }

        if (string.IsNullOrWhiteSpace(from))
        {
            throw new InvalidOperationException("SMTP 'From' address is not configured.");
        }

        var enableSsl = bool.Parse(_config["Email:Smtp:EnableSsl"] ?? "true");

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = enableSsl
        };

        var message = new MailMessage(from, toEmail, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}
