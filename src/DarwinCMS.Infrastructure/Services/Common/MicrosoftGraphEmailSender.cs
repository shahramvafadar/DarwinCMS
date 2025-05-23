// File: MicrosoftGraphEmailSender.cs
using Azure.Identity;
using DarwinCMS.Application.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace DarwinCMS.Infrastructure.Services.Common;

/// <summary>
/// Sends emails via Microsoft Graph API using client credentials and GraphServiceClient.
/// Compatible with Microsoft Graph SDK v5+.
/// </summary>
public class MicrosoftGraphEmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphClient;

    /// <summary>
    /// Initializes Microsoft GraphServiceClient with Azure Identity credentials.
    /// </summary>
    public MicrosoftGraphEmailSender(IConfiguration config)
    {
        _config = config;

        // TODO: Move these values to site-wide admin settings
        var tenantId = _config["Email:Graph:TenantId"];
        var clientId = _config["Email:Graph:ClientId"];
        var clientSecret = _config["Email:Graph:ClientSecret"];

        // Authenticate using Azure Identity (client credentials flow)
        var clientCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        // Create GraphServiceClient with required scopes
        _graphClient = new GraphServiceClient(clientCredential, new[] { "https://graph.microsoft.com/.default" });
    }

    /// <summary>
    /// Sends an email using Microsoft Graph SendMail endpoint.
    /// </summary>
    /// <param name="toEmail">Recipient's email address</param>
    /// <param name="subject">Subject of the email</param>
    /// <param name="body">HTML body content</param>
    public async Task SendAsync(string toEmail, string subject, string body)
    {
        // TODO: Move sender email address to admin-configurable settings
        var senderEmail = _config["Email:Graph:From"];

        // Construct the message body
        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = body
            },
            ToRecipients = new List<Recipient>
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = toEmail
                    }
                }
            }
        };

        // Wrap the message in SendMail request object
        var sendMailRequest = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
        {
            Message = message,
            SaveToSentItems = true
        };

        // Send the email using Microsoft Graph API
        await _graphClient.Users[senderEmail].SendMail.PostAsync(sendMailRequest);
    }
}
