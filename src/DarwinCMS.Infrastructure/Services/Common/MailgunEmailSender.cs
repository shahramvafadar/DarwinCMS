using DarwinCMS.Application.Services.Common;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DarwinCMS.Infrastructure.Services.Common;

/// <summary>
/// Sends emails using Mailgun REST API (not SMTP).
/// </summary>
public class MailgunEmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    private readonly HttpClient _http;

    /// <summary>
    /// Initializes the Mailgun sender with configuration and HttpClient.
    /// </summary>
    public MailgunEmailSender(IConfiguration config)
    {
        _config = config;
        _http = new HttpClient();
    }

    /// <summary>
    /// Sends an email using Mailgun HTTP API.
    /// </summary>
    public async Task SendAsync(string toEmail, string subject, string body)
    {
        // TODO: Move to admin-defined settings
        var domain = _config["Email:Mailgun:Domain"];
        var apiKey = _config["Email:Mailgun:ApiKey"];
        var from = _config["Email:Mailgun:From"] ?? $"no-reply@{domain}";

        var requestUri = $"https://api.mailgun.net/v3/{domain}/messages";
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", from),
            new KeyValuePair<string, string>("to", toEmail),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("html", body)
        });

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}