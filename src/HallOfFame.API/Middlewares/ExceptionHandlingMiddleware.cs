using FluentValidation;
using HallOfFame.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace HallOfFame.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) =>
        _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                result = JsonSerializer.Serialize(new { errors });
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { notFoundException.Message });
                break;
            case Exception ex:
                statusCode = HttpStatusCode.InternalServerError;
                var errorMessage = "Error: " + ex.Message;
                result = JsonSerializer.Serialize(new { errorMessage });
                break;
            default:
                result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(result);
    }
}
