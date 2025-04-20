using FluentAssertions;
using Moq;
using Plataforma.Educacao.Aluno.Application.Commands.ConcluirCurso;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var aluno = CriarAlunoComMatriculaECurtsoNaoConcluido();
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);

        var command = new ConcluirCursoCommand(aluno.Id, aluno.MatriculasCursos.Last().Id);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        _alunoRepositoryMock.Verify(r => r.AtualizarAsync(aluno), Times.Once);
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
        aluno.RegistrarHistoricoAprendizado(matriculaCursoId2, aulaId2, "Aula Teste 2", DateTime.Now.Date);

        aluno.ConcluirCurso(matriculaCursoId1);
        //aluno.ConcluirCurso(matriculaCursoId2);

        aluno.RequisitarCertificadoConclusao(matriculaCursoId1, "/caminho/certificado1.pdf");
        //aluno.RequisitarCertificadoConclusao(matriculaCursoId2, "/caminho/certificado2.pdf");

        return aluno;
    }
    #endregion
}