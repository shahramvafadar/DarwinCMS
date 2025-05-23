using DarwinCMS.Application.Services.Common;
using DarwinCMS.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models.Security;


namespace DarwinCMS.Infrastructure.Services.Common;

/// <summary>
/// Routes email sending to the appropriate backend service based on provider settings.
/// </summary>
public class SmartEmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    private readonly SmtpEmailSender _smtp;
    private readonly MailgunEmailSender _mailgun;
    private readonly MicrosoftGraphEmailSender _graph;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartEmailSender"/> class,  which provides intelligent email
    /// sending capabilities by leveraging multiple email providers.
    /// </summary>
    /// <remarks>This constructor allows the caller to provide multiple email sender implementations, enabling
    /// the <see cref="SmartEmailSender"/> to intelligently select the most appropriate provider based on  configuration
    /// settings or runtime conditions.</remarks>
    /// <param name="config">The configuration settings used to determine email sending preferences and provider-specific options.</param>
    /// <param name="smtp">An instance of <see cref="SmtpEmailSender"/> for sending emails via the SMTP protocol.</param>
    /// <param name="mailgun">An instance of <see cref="MailgunEmailSender"/> for sending emails using the Mailgun API.</param>
    /// <param name="graph">An instance of <see cref="MicrosoftGraphEmailSender"/> for sending emails through Microsoft Graph.</param>
    public SmartEmailSender(
        IConfiguration config,
        SmtpEmailSender smtp,
        MailgunEmailSender mailgun,
        MicrosoftGraphEmailSender graph)
    {
        _config = config;
        _smtp = smtp;
        _mailgun = mailgun;
        _graph = graph;
    }

    /// <summary>
    /// Selects the appropriate email provider and sends the message.
    /// </summary>
    public async Task SendAsync(string toEmail, string subject, string body)
    {
        // TODO: Load from site settings in future admin UI
        var selectedProvider = _config.GetValue<EmailProviderType>("Email:Provider");

        switch (selectedProvider)
        {
            case EmailProviderType.Smtp:
                await _smtp.SendAsync(toEmail, subject, body);
                break;

            case EmailProviderType.Mailgun:
                await _mailgun.SendAsync(toEmail, subject, body);
                break;

            case EmailProviderType.MicrosoftGraph:
                await _graph.SendAsync(toEmail, subject, body);
                break;

            default:
                throw new InvalidOperationException("Unknown email provider.");
        }
    }
}
