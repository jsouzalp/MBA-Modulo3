using Moq;
using Plataforma.Educacao.Aluno.Application.Events.PagamentoConfirmado;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Core.Messages.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plataforma.Educacao.Core.Messages;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Events;
public class PagamentoConfirmadoEventHandlerTests
{
    private readonly Mock<IAlunoRepository> _alunoRepoMock = new();
    private readonly Mock<ICursoAppService> _cursoServiceMock = new();
    private readonly Mock<IMediatorHandler> _mediatorMock = new();
    private readonly PagamentoConfirmadoEventHandler _handler;

    public PagamentoConfirmadoEventHandlerTests()
    {
        _handler = new PagamentoConfirmadoEventHandler(
            _alunoRepoMock.Object,
            _cursoServiceMock.Object,
            _mediatorMock.Object
        );
    }

    [Fact]
    public async Task Deve_atualizar_pagamento_quando_evento_valido()
    {
        var aluno = new Domain.Entities.Aluno("Teste", "teste@email.com", new DateTime(1990, 1, 1));
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId, "Curso Teste", 500);
        var matricula = aluno.MatriculasCursos.First();

        _alunoRepoMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);
        _cursoServiceMock.Setup(s => s.ObterPorIdAsync(cursoId)).ReturnsAsync(new CursoDto { Id = cursoId, Nome = "Curso Teste", CursoDisponivel = true });
        _alunoRepoMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

        var evento = new PagamentoConfirmadoEvent(matricula.Id, aluno.Id, cursoId);

        await _handler.Handle(evento, CancellationToken.None);

        _alunoRepoMock.Verify(r => r.AtualizarAsync(It.IsAny<Domain.Entities.Aluno>()), Times.Once);
        _alunoRepoMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task Deve_publicar_notificacao_quando_evento_invalido()
    {
        var evento = new PagamentoConfirmadoEvent(Guid.Empty, Guid.Empty, Guid.Empty);

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_publicar_notificacao_quando_aluno_nao_encontrado()
    {
        var evento = new PagamentoConfirmadoEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _cursoServiceMock.Setup(s => s.ObterPorIdAsync(evento.CursoId)).ReturnsAsync(new CursoDto { Id = evento.CursoId, CursoDisponivel = true });
        _alunoRepoMock.Setup(r => r.ObterPorIdAsync(evento.AlunoId)).ReturnsAsync((Domain.Entities.Aluno?)null);

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_publicar_notificacao_quando_curso_indisponivel()
    {
        var evento = new PagamentoConfirmadoEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _cursoServiceMock.Setup(s => s.ObterPorIdAsync(evento.CursoId)).ReturnsAsync((CursoDto?)null);

        await _handler.Handle(evento, CancellationToken.None);

        _mediatorMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }
}