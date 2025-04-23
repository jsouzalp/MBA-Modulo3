using Moq;
using Plataforma.Educacao.Aluno.Application.Events.PagamentoRecusado;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Core.Messages.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plataforma.Educacao.Core.Messages;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Events;
public class PagamentoRecusadoEventHandlerTests
{
    private readonly Mock<IMediatorHandler> _mediatorMock;
    private readonly PagamentoRecusadoEventHandler _handler;

    public PagamentoRecusadoEventHandlerTests()
    {
        _mediatorMock = new Mock<IMediatorHandler>();
        _handler = new PagamentoRecusadoEventHandler(_mediatorMock.Object);
    }

    [Fact]
    public async Task Deve_publicar_notificacao_quando_evento_invalido()
    {
        var evento = new PagamentoRecusadoEvent(Guid.Empty, Guid.Empty, Guid.Empty, string.Empty);

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(
            It.Is<DomainNotificacaoRaiz>(n => n.RaizAgregacao == evento.RaizAgregacao)), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Nao_deve_publicar_nada_quando_evento_valido()
    {
        var evento = new PagamentoRecusadoEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Falha no pagamento");

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);
    }
}