using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Project_C_Sharp.Shared.Errors;
using Project_C_Sharp.Shared.Exceptions;
using Microsoft.Extensions.Hosting;

namespace Project_C_Sharp.Shared.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    private readonly IWebHostEnvironment _env;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var error = new ApiErrorResponse
        {
            TraceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
            Message = context.Exception.Message,
            StatusCode = GetStatusCode(context.Exception)
        };

        // Log completo apenas em desenvolvimento
        if (_env.IsDevelopment())
        {
            _logger.LogError(context.Exception, "Erro na aplicação: {Message}", context.Exception.Message);
        }
        else
        {
            _logger.LogError("Erro na aplicação: {Message}", context.Exception.Message);
        }

        context.Result = new ObjectResult(error)
        {
            StatusCode = error.StatusCode
        };

        context.ExceptionHandled = true;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
}