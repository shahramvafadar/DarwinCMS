namespace DarwinCMS.Application.Services.Common;

/// <summary>
/// Defines a contract for sending email messages.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email message to the specified recipient.
    /// </summary>
    /// <param name="toEmail">Target email address</param>
    /// <param name="subject">Subject line of the email</param>
    /// <param name="body">HTML or plain-text content of the message</param>
    Task SendAsync(string toEmail, string subject, string body);
}
