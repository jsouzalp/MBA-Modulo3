using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Api.Enumerators;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Plataforma.Educacao.Api.Filters;
public class DomainExceptionFilter : IExceptionFilter
{
    private readonly IActionResultExecutor<ObjectResult> _executor;
    private readonly ILogger _logger;

    public DomainExceptionFilter(IActionResultExecutor<ObjectResult> executor, ILogger<ExceptionFilter> logger)
    {
        _executor = executor;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException ex)
        {
            context.ExceptionHandled = true;
            _logger.LogError(context?.Exception ?? context.Exception, $"Ocorreu um erro de DOMINIO: {context?.Exception?.Message ?? context.Exception?.ToString()}");

            ObjectResult output;
            var outputResponse = new
            {
                success = false,
                type = ResponseTypeEnum.DomainError.ToString(),
                errors = new[] { ex.Message }
            };

            output = new ObjectResult(outputResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Value = outputResponse
            };

            context.ExceptionHandled = true;

            _executor.ExecuteAsync(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor), output)
                .GetAwaiter()
                .GetResult();
        }
    }
}