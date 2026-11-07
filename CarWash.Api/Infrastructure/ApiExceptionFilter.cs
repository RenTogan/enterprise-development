using CarWash.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarWash.Api.Infrastructure;

/// <summary>
/// Перехват исключений и преоб-ие в читаемый ProblemDetails
/// </summary>
public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        (var statusCode, var title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Ресурс не найден"),
            BadRequestException => (StatusCodes.Status400BadRequest, "Некорректный запрос"),
            ConflictException => (StatusCodes.Status409Conflict, "конфликт данных"),
            _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Необработанная ошибка");
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "Произошла непредвиденная ошибка на сервере"
                : exception.Message,
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(problem) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }
}