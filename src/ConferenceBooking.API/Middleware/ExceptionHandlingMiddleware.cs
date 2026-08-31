using System.Net;
using System.Text.Json;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.API.Middleware
{
    /// <summary>Перехоплює domain-виключення та повертає відповідні HTTP коди.</summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                RoomNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                BookingConflictException => (HttpStatusCode.Conflict, exception.Message),
                InvalidBookingException => (HttpStatusCode.BadRequest, exception.Message),
                DomainException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Внутрішня помилка сервера.")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Unhandled exception");
            else
                _logger.LogWarning("Domain exception: {Message}", exception.Message);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = JsonSerializer.Serialize(new
            {
                error = message,
                statusCode = (int)statusCode
            });

            await context.Response.WriteAsync(response);
        }
    }
}
