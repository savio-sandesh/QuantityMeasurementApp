using System.Net;
using System.Text.Json;
using BusinessLayer;
using QuantityMeasurementWebApi.Models;

namespace QuantityMeasurementWebApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var isDomainException = exception is QuantityMeasurementException;
            var statusCode = isDomainException
                ? (int)HttpStatusCode.BadRequest
                : (int)HttpStatusCode.InternalServerError;

            var error = new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                Status = statusCode,
                Error = isDomainException ? "Bad Request" : "Internal Server Error",
                Message = exception.Message,
                Path = context.Request.Path
            };

            var payload = JsonSerializer.Serialize(error);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}
