using FluentAssertions;
using Moq;
using Plataforma.Educacao.Aluno.Application.Commands.RegistrarHistoricoAprendizado;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Commands;

public class RegistrarHistoricoAprendizadoCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly RegistrarHistoricoAprendizadoCommandHandler _handler;

    public RegistrarHistoricoAprendizadoCommandHandlerTests()
    {
        _alunoRepositoryMock = new Mock<IAlunoRepository>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        _handler = new RegistrarHistoricoAprendizadoCommandHandler(
            _alunoRepositoryMock.Object,
            _mediatorHandlerMock.Object
        );
    }

    [Fact]
    public async Task Deve_retornar_false_quando_requisicao_invalida()
    {
        // Arrange
        var command = new RegistrarHistoricoAprendizadoCommand(Guid.Empty, Guid.Empty, Guid.Empty, null, null);

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
        var command = CriarComandoValido();

        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Aluno)null);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_lancar_excecao_quando_matricula_invalida()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(aluno);

        var command = CriarComandoValido();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*Matrícula não foi localizada*");
    }

    [Fact]
    public async Task Deve_registrar_historico_com_sucesso()
    {
        // Arrange
        var aluno = CriarAlunoComMatriculaECurtsoNaoConcluido();
        var matricula = aluno.MatriculasCursos.First();
        Guid aulaId = Guid.NewGuid();

        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);

        var cursoDto = new CursoDto
        {
            Id = Guid.NewGuid(),
            CursoDisponivel = true,
            Aulas = new List<AulaDto> { new AulaDto { Id = aulaId, Descricao = "Aula Teste", Ativo = true } }
        };

        var command = new RegistrarHistoricoAprendizadoCommand(aluno.Id, matricula.Id, aulaId, cursoDto, null);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        //_alunoRepositoryMock.Verify(r => r.AtualizarAsync(aluno), Times.Once);
    }

    #region Helpers

    private static RegistrarHistoricoAprendizadoCommand CriarComandoValido()
    {
        var matriculaId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        var cursoDto = new CursoDto
        {
            Id = Guid.NewGuid(),
            CursoDisponivel = true,
            Aulas = new List<AulaDto> { new AulaDto { Id = aulaId, Descricao = "Aula Teste", Ativo = true } }
        };

        return new RegistrarHistoricoAprendizadoCommand(Guid.NewGuid(), matriculaId, aulaId, cursoDto, DateTime.UtcNow);
    }

    private static Domain.Entities.Aluno CriarAlunoValido()
    {
        var aluno = new Domain.Entities.Aluno("Aluno Teste", "teste@email.com", new DateTime(1990, 1, 1));
        aluno.MatricularEmCurso(Guid.NewGuid(), "Curso Teste", 500);
        return aluno;
    }

    private static Domain.Entities.Aluno CriarAlunoComMatriculaECurtsoNaoConcluido()
    {
        Guid cursoId1 = Guid.NewGuid();
        Guid aulaId1 = Guid.NewGuid();
        Guid cursoId2 = Guid.NewGuid();
        Guid aulaId2 = Guid.NewGuid();

        var aluno = new Domain.Entities.Aluno("Aluno Teste", "teste@email.com", new DateTime(1995, 1, 1));
        aluno.MatricularEmCurso(cursoId1, "Curso Teste", 100);
        aluno.MatricularEmCurso(cursoId2, "Outro Curso Teste", 200);

        Guid matriculaCursoId1 = aluno.MatriculasCursos.First().Id;
        Guid matriculaCursoId2 = aluno.MatriculasCursos.Last().Id;

        aluno.AtualizarPagamentoMatricula(matriculaCursoId1);
        aluno.AtualizarPagamentoMatricula(matriculaCursoId2);

        aluno.RegistrarHistoricoAprendizado(matriculaCursoId1, aulaId1, "Aula Teste 1", null);
        aluno.RegistrarHistoricoAprendizado(matriculaCursoId2, aulaId2, "Aula Teste 2", null);

        aluno.RegistrarHistoricoAprendizado(matriculaCursoId1, aulaId1, "Aula Teste 1", DateTime.Now.Date);
        //aluno.RegistrarHistoricoAprendizado(matriculaCursoId2, aulaId2, "Aula Teste 2", DateTime.Now.Date);

        //aluno.ConcluirCurso(matriculaCursoId1);
        ////aluno.ConcluirCurso(matriculaCursoId2);

        //aluno.RequisitarCertificadoConclusao(matriculaCursoId1, "/caminho/certificado1.pdf");
        ////aluno.RequisitarCertificadoConclusao(matriculaCursoId2, "/caminho/certificado2.pdf");

        return aluno;
    }
    #endregion
}