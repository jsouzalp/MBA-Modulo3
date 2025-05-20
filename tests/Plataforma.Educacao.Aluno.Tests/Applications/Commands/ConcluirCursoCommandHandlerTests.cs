using FluentAssertions;
using Moq;
using Plataforma.Educacao.Aluno.Application.Commands.ConcluirCurso;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Commands;
public class ConcluirCursoCommandHandlerTests
{

    private readonly Mock<ICursoAppService> _cursoServiceMock;
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly ConcluirCursoCommandHandler _handler;

    public ConcluirCursoCommandHandlerTests()
    {
        _cursoServiceMock = new Mock<ICursoAppService>();
        _alunoRepositoryMock = new Mock<IAlunoRepository>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        _handler = new ConcluirCursoCommandHandler(
            _alunoRepositoryMock.Object,
            _cursoServiceMock.Object,
            _mediatorHandlerMock.Object
        );
    }

    [Fact]
    public async Task Deve_retornar_false_quando_requisicao_invalida()
    {
        // Arrange
        var command = new ConcluirCursoCommand(Guid.Empty, Guid.Empty);

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
    public async Task Deve_concluir_curso_com_sucesso()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        _cursoServiceMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync(new CursoDto { Id = cursoId, Nome = "Curso de DDD do básico ao avançado", Valor = 1000, CursoDisponivel = true });

        var aluno = CriarAlunoComMatriculaECurtsoPorConcluir(cursoId);
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);

        var command = new ConcluirCursoCommand(aluno.Id, aluno.MatriculasCursos.Last().Id);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        _alunoRepositoryMock.Verify(r => r.AtualizarAsync(aluno), Times.Once);
    }

    [Fact]
    public async Task Deve_retornar_false_quando_nao_existem_aulas_ativas()
    {
        var cursoId = Guid.NewGuid();
        var curso = new CursoDto
        {
            Id = cursoId,
            Nome = "Curso Teste",
            Valor = 500,
            CursoDisponivel = true,
            Aulas = new List<AulaDto> { new() { Id = Guid.NewGuid(), Descricao = "Aula de refatoração 01", Ativo = false } }
        };


        var aluno = CriarAlunoComMatriculaECurtsoConcluido(cursoId);
        _cursoServiceMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync(curso);
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);

        var command = new ConcluirCursoCommand(aluno.Id, aluno.MatriculasCursos.Last().Id);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    #region Helpers

    private static ConcluirCursoCommand CriarComandoValido()
    {
        return new ConcluirCursoCommand(Guid.NewGuid(), Guid.NewGuid());
    }

    private static Domain.Entities.Aluno CriarAlunoValido()
    {
        var aluno = new Domain.Entities.Aluno("Aluno Teste", "teste@email.com", new DateTime(1990, 1, 1));
        aluno.MatricularEmCurso(Guid.NewGuid(), "Curso Teste", 500);
        return aluno;
    }

    private static Domain.Entities.Aluno CriarAlunoComMatriculaECurtsoConcluido(Guid cursoId)
    {
        Guid aulaId1 = Guid.NewGuid();

        var aluno = new Domain.Entities.Aluno("Aluno Teste", "teste@email.com", new DateTime(1995, 1, 1));
        aluno.MatricularEmCurso(cursoId, "Curso Teste", 100);

        Guid matriculaCursoId1 = aluno.MatriculasCursos.First().Id;

        aluno.AtualizarPagamentoMatricula(matriculaCursoId1);
        aluno.RegistrarHistoricoAprendizado(matriculaCursoId1, aulaId1, "Aula Teste 1", null);
        aluno.RegistrarHistoricoAprendizado(matriculaCursoId1, aulaId1, "Aula Teste 1", DateTime.Now.Date);
        aluno.ConcluirCurso(matriculaCursoId1);

        aluno.RequisitarCertificadoConclusao(matriculaCursoId1, "/caminho/certificado1.pdf");

        return aluno;
    }

    private static Domain.Entities.Aluno CriarAlunoComMatriculaECurtsoPorConcluir(Guid cursoId)
    {
        Guid aulaId1 = Guid.NewGuid();
        var aluno = new Domain.Entities.Aluno("Aluno Teste", "teste@email.com", new DateTime(1995, 1, 1));
        aluno.MatricularEmCurso(cursoId, "Curso Teste", 100);
        Guid matriculaCursoId1 = aluno.MatriculasCursos.First().Id;
        aluno.AtualizarPagamentoMatricula(matriculaCursoId1);
        aluno.RegistrarHistoricoAprendizado(matriculaCursoId1, aulaId1, "Aula Teste 1", DateTime.Now.Date);
        return aluno;
    }

    #endregion
}