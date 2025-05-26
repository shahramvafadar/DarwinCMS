using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;

namespace DarwinCMS.WebAdmin.Infrastructure.Middleware;

/// <summary>
/// Middleware to globally handle unhandled exceptions in the request pipeline.
/// Logs the exception and returns a standardized error response.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Initializes the global exception middleware.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger to write exception details to output.</param>
    /// <param name="env">Current hosting environment for development/production behavior.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Executes the middleware logic, catching unhandled exceptions and writing a JSON error response.
    /// </summary>
    /// <param name="context">The HTTP context of the request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            if (ex is BusinessRuleException bre)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new List<UiMessage>
                {
                    UiMessage.CreateError(bre.Message)
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            var errorId = Guid.NewGuid();
            _logger.LogError(ex, "Unhandled exception occurred. ErrorId: {ErrorId}", errorId);

            if (_env.IsDevelopment())
            {
                throw;
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var fallback = new List<UiMessage>
            {
                UiMessage.CreateError("A system error occurred. Please contact support.", errorId.ToString())
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(fallback));
        }
    }
}
