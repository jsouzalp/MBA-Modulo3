using MediatR;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoEvents;

namespace Plataforma.Educacao.Aluno.Application.Events.ProblemaRegistroHistoricoAprendizado;
public class RegistrarProblemaHistoricoAprendizadoEventHandler(IMediatorHandler mediatorHandler) : INotificationHandler<RegistrarProblemaHistoricoAprendizadoEvent>
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task Handle(RegistrarProblemaHistoricoAprendizadoEvent notification, CancellationToken cancellationToken)
    {
        // O que fazer aqui?
        // Gravar o problema no banco de dados?
        // Enviar um e-mail para o suporte?
        // Enviar uma notificação para o usuário?
        // Registrar o problema em um log?
        // Não sei o que fazer!!!

        _raizAgregacao = notification.RaizAgregacao;
        if (!ValidarRequisicao(notification)) { return; }
    }

    private bool ValidarRequisicao(RegistrarProblemaHistoricoAprendizadoEvent notification)
    {
        notification.DefinirValidacao(new RegistrarProblemaHistoricoAprendizadoEventValidator().Validate(notification));
        if (!notification.EhValido())
        {
            foreach (var erro in notification.Erros)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        return true;
    }
}