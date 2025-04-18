using MediatR;

namespace Plataforma.Educacao.Core.Messages.Comunications;
public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;
    //private readonly IEventSourcingRepository _eventSourcingRepository;

    //public MediatorHandler(IMediator mediator, IEventSourcingRepository eventSourcingRepository)
    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
        //_eventSourcingRepository = eventSourcingRepository;
    }

    //public async Task PublicarEvento<T>(T evento) where T : Event
    //{
    //    await _mediator.Publish(evento);
    //    await _eventSourcingRepository.SalvarEvento(evento);

    //}

    public async Task<bool> EnviarComando<T>(T comando) where T : CommandRaiz
    {
        return await _mediator.Send(comando);
    }

    public async Task PublicarNotificacaoDominio<T>(T notificacao) where T : DomainNotificacaoRaiz
    {
        await _mediator.Publish(notificacao);
    }

    //public async Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent
    //{
    //    await _mediator.Publish(notificacao);
    //}
}
