using FluentAssertions;
using Moq;
using Plataforma.Educacao.Aluno.Application.Queries;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Queries;
public class AlunoQueryServiceTests
{
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly AlunoQueryService _service;

    public AlunoQueryServiceTests()
    {
        _alunoRepositoryMock = new Mock<IAlunoRepository>();
        _service = new AlunoQueryService(_alunoRepositoryMock.Object);
    }

    [Fact]
    public async Task Deve_retornar_null_quando_aluno_nao_encontrado()
    {
        // Arrange
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Aluno)null);

        // Act
        var aluno = await _service.ObterAlunoPorIdAsync(Guid.NewGuid());

        // Assert
        aluno.Should().BeNull();
    }

    [Fact]
    public async Task Deve_retornar_lista_vazia_quando_matriculas_nao_existirem()
    {
        // Arrange
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Aluno)null);

        // Act
        var matriculas = await _service.ObterMatriculasPorAlunoIdAsync(Guid.NewGuid());

        // Assert
        matriculas.Should().BeEmpty();
    }

    [Fact]
    public async Task Deve_retornar_null_quando_certificado_nao_existir()
    {
        // Arrange
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((MatriculaCurso)null);

        // Act
        var certificado = await _service.ObterCertificadoPorMatriculaIdAsync(Guid.NewGuid());

        // Assert
        certificado.Should().BeNull();
    }

    [Fact]
    public async Task Deve_retornar_aluno_mapeado_quando_existir()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);

        // Act
        var result = await _service.ObterAlunoPorIdAsync(aluno.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(aluno.Id);
        result.Nome.Should().Be(aluno.Nome);
        result.Email.Should().Be(aluno.Email);
        result.DataNascimento.Should().Be(aluno.DataNascimento);
    }

    [Fact]
    public async Task Deve_retornar_matriculas_mapeadas_quando_existirem()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(aluno.Id)).ReturnsAsync(aluno);

        // Act
        var matriculas = await _service.ObterMatriculasPorAlunoIdAsync(aluno.Id);

        // Assert
        matriculas.Should().NotBeEmpty();
        matriculas.First().CursoId.Should().Be(aluno.MatriculasCursos.First().CursoId);
    }

    [Fact]
    public async Task Deve_retornar_certificado_mapeado_quando_existir()
    {
        // Arrange
        var matricula = CriarAlunoValido().MatriculasCursos.First();
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorIdAsync(matricula.Id)).ReturnsAsync(matricula);

        // Act
        var certificado = await _service.ObterCertificadoPorMatriculaIdAsync(matricula.Id);

        // Assert
        certificado.Should().NotBeNull();
        certificado.PathCertificado.Should().Be(matricula.Certificado.PathCertificado);
    }

    #region Helpers

    private static Domain.Entities.Aluno CriarAlunoValido()
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
        aluno.ConcluirCurso(matriculaCursoId2);

        aluno.RequisitarCertificadoConclusao(matriculaCursoId1, "/caminho/certificado1.pdf");
        aluno.RequisitarCertificadoConclusao(matriculaCursoId2, "/caminho/certificado2.pdf");

        return aluno;
    }

    //private static MatriculaCurso CriarMatriculaComCertificado()
    //{
    //    var matricula = new MatriculaCurso(Guid.NewGuid(), "Curso Teste", 200.00m);
    //    matricula.RequisitarCertificadoConclusao("/caminho/certificado.pdf");
    //    return matricula;
    //}

    #endregion
}