namespace DarwinCMS.Domain.Enums;

/// <summary>
/// Enumerates supported email sending methods.
/// </summary>
public enum EmailProviderType
{
    /// <summary>
    /// Uses standard SMTP protocol to send emails.
    /// </summary>
    Smtp = 1,

    /// <summary>
    /// Uses the Mailgun HTTP API to send transactional emails.
    /// </summary>
    Mailgun = 2,

    /// <summary>
    /// Uses Microsoft Graph API to send emails from Microsoft 365 accounts.
    /// </summary>
    MicrosoftGraph = 3
}

