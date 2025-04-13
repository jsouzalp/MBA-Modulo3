using FluentAssertions;
using Moq;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Services;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Conteudo.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Application.Tests;
public class AulaAppServiceTests
{
    #region Helpers
    private Curso CriarCurso(string nomeCurso = "Curso de DDD")
    {
        return new Curso(
            nomeCurso,
            1000m,
            DateTime.Now.AddMonths(6),
            new ConteudoProgramatico("Finalidade", new string('A', 60))
        );
    }

    private AulaDto CriarAulaDto() => new AulaDto
    {
        Descricao = "Aula de Introdução",
        CargaHoraria = 2,
        OrdemAula = 1,
        Ativo = true
    };

    private AulaAppService CriarAppService(out Mock<ICursoRepository> cursoRepoMock, Curso curso)
    {
        cursoRepoMock = new Mock<ICursoRepository>();
        cursoRepoMock.Setup(r => r.ObterPorIdAsync(curso.Id)).ReturnsAsync(curso);
        cursoRepoMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

        return new AulaAppService(cursoRepoMock.Object);
    }
    #endregion

    [Fact]
    public async Task Deve_adicionar_aula_com_sucesso()
    {
        // Arrange
        var curso = CriarCurso();
        var aulaDto = CriarAulaDto();
        var service = CriarAppService(out var mock, curso);

        // Act
        var aulaId = await service.AdicionarAulaAsync(curso.Id, aulaDto);

        // Assert
        curso.Aulas.Should().ContainSingle();
        aulaId.Should().NotBe(Guid.Empty);
        mock.Verify(r => r.AtualizarAsync(curso), Times.Once);
        mock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task Deve_atualizar_aula_com_sucesso()
    {
        // Arrange
        var curso = CriarCurso();
        curso.AdicionarAula("Aula Original", 1, 1);
        var aula = curso.Aulas.First();

        var dto = new AulaDto
        {
            Id = aula.Id,
            Descricao = "Aula Atualizada",
            CargaHoraria = 3,
            OrdemAula = 2,
            Ativo = false
        };

        var service = CriarAppService(out var mock, curso);

        // Act
        await service.AtualizarAulaAsync(curso.Id, dto);

        // Assert
        aula.Descricao.Should().Be("Aula Atualizada");
        aula.CargaHoraria.Should().Be(3);
        aula.OrdemAula.Should().Be(2);
        aula.Ativo.Should().BeFalse();
    }

    [Fact]
    public async Task Deve_remover_aula_com_sucesso()
    {
        // Arrange
        var curso = CriarCurso();
        curso.AdicionarAula("Aula a remover", 1, 1);
        var aula = curso.Aulas.First();
        var service = CriarAppService(out var mock, curso);

        // Act
        await service.RemoverAulaAsync(curso.Id, aula.Id);

        // Assert
        curso.Aulas.Should().BeEmpty();
        mock.Verify(r => r.AtualizarAsync(curso), Times.Once);
        mock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task Deve_lancar_excecao_quando_aula_nao_encontrada()
    {
        // Arrange
        var curso = CriarCurso();
        var service = CriarAppService(out _, curso);
        var dto = CriarAulaDto();
        dto.Id = Guid.NewGuid(); // ID inexistente

        // Act
        Func<Task> act = async () => await service.AtualizarAulaAsync(curso.Id, dto);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Aula não encontrada");
    }
}