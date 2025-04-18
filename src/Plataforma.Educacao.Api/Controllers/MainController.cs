using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Plataforma.Educacao.Api.Enumerators;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Handlers;
using Plataforma.Educacao.Core.Messages;
using System.Net;

namespace Plataforma.Educacao.Api.Controllers;

[ApiController]
public class MainController(INotificationHandler<DomainNotificacaoRaiz> notifications, 
    IMediatorHandler mediatorHandler) : ControllerBase
{
    //private readonly IAppIdentityUser _appIdentityUser;
    //private readonly INotificationService _notificationService;

    //public Guid UserId => _appIdentityUser.GetUserId();
    //public bool IsAuthenticated => _appIdentityUser.IsAuthenticated();
    //public string UserEmail => _appIdentityUser.GetUserEmail();

    //protected MainController(IAppIdentityUser appIdentityUser, INotificationService notificationService)
    //{
    //    _appIdentityUser = appIdentityUser;
    //    _notificationService = notificationService;
    //}

    protected Guid UserId => Guid.NewGuid(); // _appIdentityUser.GetUserId();

    protected readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    protected readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    protected bool OperacaoValida() => !_notifications.TemNotificacao();

    protected ActionResult GenerateResponse(object? result = null,
        ResponseTypeEnum responseType = ResponseTypeEnum.Success,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        IList<string> errors = null)
    {
        if (OperacaoValida() && ((int)statusCode >= 200 && (int)statusCode <= 299))
        {
            return new JsonResult(new
            {
                success = true,
                type = responseType.ToString(),
                result
            })
            {
                StatusCode = (int)statusCode
            };
        }

        errors ??= [];
        if (_notifications.TemNotificacao())
        {
            var notificationErrors = _notifications.ObterNotificacoes().Select(n => $"Chave: {n.Chave} Mensagem: {n.Valor}").ToList();
            foreach(string erro in notificationErrors)
            {
                errors.Add(erro);
            }
        }

        return new JsonResult(new
        {
            success = false,
            type = responseType.ToString(),
            errors
        })
        {
            StatusCode = (int)statusCode
        };
    }

    protected ActionResult GenerateModelStateResponse(ResponseTypeEnum responseType, HttpStatusCode statusCode, ModelStateDictionary modelState)
    {
        return new JsonResult(new
        {
            success = false,
            type = responseType.ToString(),
            errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        })
        {
            StatusCode = (int)statusCode
        };
    }
}