using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;

namespace DarwinCMS.WebAdmin.Infrastructure.Middleware;

/// <summary>
/// Middleware to globally handle unhandled exceptions in the request pipeline.
/// Logs the exception and returns a standardized JSON error response to the client.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Initializes the global exception middleware with required dependencies.
    /// </summary>
    /// <param name="next">The next middleware to invoke.</param>
    /// <param name="logger">Logger to capture error details (e.g., Serilog).</param>
    /// <param name="env">Hosting environment for environment-specific behavior.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Executes the middleware logic, catching unhandled exceptions and returning a standardized JSON error response.
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

            // Handle known business rule exceptions separately with 400 BadRequest
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

            // Generate a unique error ID for traceability
            var errorId = Guid.NewGuid();

            // Log the exception with full context and error ID
            _logger.LogError(ex, "Unhandled exception occurred. ErrorId: {ErrorId}, Path: {Path}, Query: {QueryString}",
                errorId,
                context.Request.Path,
                context.Request.QueryString);

            // In development, re-throw the exception to see full stack trace in the browser
            if (_env.IsDevelopment())
            {
                throw;
            }

            // In production, return a generic system error message with the reference ID
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var fallback = new List<UiMessage>
            {
                UiMessage.CreateError("A system error occurred. Please contact support.", errorId.ToString())
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(fallback));
        }
    }
}
