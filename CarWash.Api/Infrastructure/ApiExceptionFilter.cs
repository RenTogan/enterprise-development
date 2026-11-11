using CarWash.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CarWash.Api.Infrastructure;

/// <summary>
/// Перехват исключений и преобразование в читаемый ProblemDetails
/// </summary>
public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        (var statusCode, var title, var detail) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Ресурс не найден", exception.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, "Некорректный запрос", exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, "Конфликт данных", exception.Message),
            DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx => HandlePostgresException(pgEx),
            DbUpdateException => (StatusCodes.Status409Conflict, "Ошибка сохранения данных", "Не удалось сохранить изменения из-за конфликта данных."),
            _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера", "Произошла непредвиденная ошибка на сервере")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Необработанная ошибка");
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(problem) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }

    private static (int StatusCode, string Title, string Detail) HandlePostgresException(PostgresException pgEx) => pgEx.SqlState switch
    {
        PostgresErrorCodes.UniqueViolation => (StatusCodes.Status409Conflict, "Конфликт уникальности", "Запись с такими уникальными данными уже существует."),
        PostgresErrorCodes.ForeignKeyViolation => (StatusCodes.Status409Conflict, "Конфликт внешнего ключа", "Операция нарушает ссылочную целостность данных."),
        _ => (StatusCodes.Status400BadRequest, "Ошибка базы данных", pgEx.MessageText)
    };
}