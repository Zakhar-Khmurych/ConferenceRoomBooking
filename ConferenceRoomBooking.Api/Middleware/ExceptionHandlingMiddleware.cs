using System.Text.Json;
using ConferenceRoomBooking.Application.Exceptions;

namespace ConferenceRoomBooking.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception.");

            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException =>
                StatusCodes.Status404NotFound,

            BookingConflictException =>
                StatusCodes.Status409Conflict,

            ValidationException =>
                StatusCodes.Status400BadRequest,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            status = statusCode,
            title = GetTitle(statusCode),
            detail = statusCode == 500
                ? "An unexpected error occurred."
                : exception.Message
        };

        context.Response.StatusCode = statusCode;

        context.Response.ContentType =
            "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            400 => "Validation error",
            404 => "Resource not found",
            409 => "Conflict",
            _ => "Internal server error"
        };
    }
}