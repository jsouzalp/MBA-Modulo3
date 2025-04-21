using MediatR;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;

namespace Plataforma.Educacao.Faturamento.Application.Events.GerarLinkPagamento;
public class GerarLinkPagamentoEventHandler(IFaturamentoRepository faturamentoRepository,
    IMediatorHandler mediatorHandler) : INotificationHandler<GerarLinkPagamentoEvent>
{
    private readonly IFaturamentoRepository _faturamentoRepository = faturamentoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task Handle(GerarLinkPagamentoEvent request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return; }

        // O que fazer aqui?
        // Chamo um proxy de pagamento?
        // Envio um email com o link de pagamento?
        // Devo enviar o link de pagamento para algum BC?
        // Registrar em um log?
        // Não sei o que fazer!!!

        var pagamento = new Pagamento(request.MatriculaCursoId, request.Valor, request.DataHora.AddDays(7).Date);
        await _faturamentoRepository.AdicionarAsync(pagamento);

        await _faturamentoRepository.UnitOfWork.Commit();
    }

    private bool ValidarRequisicao(GerarLinkPagamentoEvent notification)
    {
        notification.DefinirValidacao(new GerarLinkPagamentoEventValidator().Validate(notification));
        if (!notification.EhValido())
        {
            foreach (var erro in notification.Erros)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        return true;
    }
}
