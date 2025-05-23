using DarwinCMS.Application.Services.Common;
using System.Diagnostics;

namespace DarwinCMS.Infrastructure.Services.Common;

/// <summary>
/// Simple debug email sender that logs the message to the output or console.
/// Used in development or non-production environments.
/// </summary>
public class DebugEmailSender : IEmailSender
{
    /// <summary>
    /// Writes the email contents to the debug output or console.
    /// </summary>
    public Task SendAsync(string toEmail, string subject, string body)
    {
        var message = $"""
        === [DEBUG EMAIL SENT] ===
        To:      {toEmail}
        Subject: {subject}

        Body:
        {body}
        ===========================
        """;

        Debug.WriteLine(message);
        Console.WriteLine(message);

        return Task.CompletedTask;
    }
}
