using Moq;
using Plataforma.Educacao.Aluno.Application.Events.ProblemaRegistroHistoricoAprendizado;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoEvents;
using Plataforma.Educacao.Core.Messages.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plataforma.Educacao.Core.Messages;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Events;
public class RegistrarProblemaHistoricoAprendizadoEventHandlerTests
{
    private readonly Mock<IMediatorHandler> _mediatorMock;
    private readonly RegistrarProblemaHistoricoAprendizadoEventHandler _handler;

    public RegistrarProblemaHistoricoAprendizadoEventHandlerTests()
    {
        _mediatorMock = new Mock<IMediatorHandler>();
        _handler = new RegistrarProblemaHistoricoAprendizadoEventHandler(_mediatorMock.Object);
    }

    [Fact]
    public async Task Deve_publicar_notificacao_quando_evento_invalido()
    {
        var evento = new RegistrarProblemaHistoricoAprendizadoEvent(Guid.Empty, Guid.Empty, Guid.Empty, DateTime.Today, "Algum erro aconteceu...");

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(
            It.Is<DomainNotificacaoRaiz>(n => n.RaizAgregacao == evento.RaizAgregacao)), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Nao_deve_publicar_nada_quando_evento_valido()
    {
        var evento = new RegistrarProblemaHistoricoAprendizadoEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Today, "Motivo Teste");

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);
    }
}