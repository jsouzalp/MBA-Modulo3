using FluentAssertions;
using Moq;
using Plataforma.Educacao.Aluno.Application.Commands.AtualizarPagamento;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Commands;

public class AtualizarPagamentoMatriculaCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Mock<ICursoAppService> _cursoServiceMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly AtualizarPagamentoMatriculaCommandHandler _handler;

    public AtualizarPagamentoMatriculaCommandHandlerTests()
    {
        _alunoRepositoryMock = new Mock<IAlunoRepository>();
        _cursoServiceMock = new Mock<ICursoAppService>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        _handler = new AtualizarPagamentoMatriculaCommandHandler(
            _alunoRepositoryMock.Object,
            _cursoServiceMock.Object,
            _mediatorHandlerMock.Object
        );
    }

    [Fact]
    public async Task Deve_retornar_false_quando_requisicao_invalida()
    {
        // Arrange
        var command = new AtualizarPagamentoMatriculaCommand(Guid.Empty, Guid.Empty);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_retornar_false_quando_curso_indisponivel()
    {
        // Arrange
        var command = new AtualizarPagamentoMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        _cursoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((CursoDto)null);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_retornar_false_quando_aluno_nao_encontrado()
    {
        // Arrange
        var command = new AtualizarPagamentoMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        _cursoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CursoDto { Id = Guid.NewGuid(), CursoDisponivel = true });
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Aluno)null);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_atualizar_pagamento_matricula_com_sucesso()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var aluno = new Domain.Entities.Aluno("Aluno Teste", "teste@email.com", new DateTime(1990, 1, 1));
        aluno.MatricularEmCurso(cursoId, "Curso Teste", 500);

        _cursoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CursoDto { Id = cursoId, CursoDisponivel = true });
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync(aluno);

        var command = new AtualizarPagamentoMatriculaCommand(alunoId, cursoId);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        aluno.MatriculasCursos.First().EstadoMatricula.Should().Be(Domain.Enumerators.EstadoMatriculaCursoEnum.PagamentoRealizado);
        _alunoRepositoryMock.Verify(r => r.AtualizarAsync(aluno), Times.Once);
    }
}