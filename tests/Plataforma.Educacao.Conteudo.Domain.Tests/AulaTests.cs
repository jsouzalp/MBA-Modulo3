using FluentAssertions;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Exceptions;

namespace Plataforma.Educacao.Conteudo.Domain.Tests;
public class AulaTests
{
    #region Helpers
    private const string _cursoIdInvalido = "00000000-0000-0000-0000-000000000000";
    private const string _cursoIdValido = "11111111-1111-1111-1111-111111111111";
    private const string _descricaoValida = "Introdução ao Domain Driven Design";
    private const int _cargaHorariaValida = 4;
    private const byte _ordemAulaValida = 1;

    private Aula CriarInstanciaAula(string cursoId = _cursoIdValido, 
        string descricao = _descricaoValida, 
        short cargaHoraria = _cargaHorariaValida, 
        byte ordemAula = _ordemAulaValida)
    {
        return new Aula(Guid.Parse(cursoId), descricao, cargaHoraria, ordemAula);
    }
    #endregion

    #region Construtores
    [Fact]
    public void Deve_criar_aula_valida()
    {
        // Act
        var aula = CriarInstanciaAula();

        // Assert
        aula.Should().NotBeNull();
        aula.Descricao.Should().Be(_descricaoValida);
        aula.CargaHoraria.Should().Be(_cargaHorariaValida);
        aula.OrdemAula.Should().Be(_ordemAulaValida);
        aula.CursoId.Should().Be(Guid.Parse(_cursoIdValido));
    }

    [Theory]
    [InlineData(_cursoIdInvalido, _descricaoValida, _cargaHorariaValida, _ordemAulaValida, "*Id do curso não pode ser vazio*")]
    [InlineData("11111111-1111-1111-1111-111111111111", "", _cargaHorariaValida, _ordemAulaValida, "*Descrição da aula não pode ser vazia ou nula*")]
    [InlineData(_cursoIdValido, "abc", _cargaHorariaValida, _ordemAulaValida, "*Descrição da aula deve ter entre 5 e 100 caracteres*")]
    [InlineData(_cursoIdValido, _descricaoValida, 0, _ordemAulaValida, "*Carga horária deve ser maior que zero*")]
    [InlineData(_cursoIdValido, _descricaoValida, 10, _ordemAulaValida, "*Carga horária deve estar entre 1 e 5 horas*")]
    [InlineData(_cursoIdValido, _descricaoValida, 2, 0, "*Ordem da aula deve ser maior que zero*")]
    public void Nao_deve_criar_aula_invalida(string cursoId, string descricao, short cargaHoraria, byte ordemAula, string mensagemErro)
    {
        // Arrange

        // Act
        Action act = () => CriarInstanciaAula(cursoId, descricao, cargaHoraria, ordemAula);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }
    #endregion

    #region Setters
    [Fact]
    public void Deve_ativar_e_desativar_aula()
    {
        var aula = CriarInstanciaAula();

        aula.AtivarAula();
        aula.Ativo.Should().BeTrue();

        aula.DesativarAula();
        aula.Ativo.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "*Descrição da aula não pode ser vazia ou nula*")]
    [InlineData("abc", "*Descrição da aula deve ter entre 5 e 100 caracteres*")]
    public void Nao_deve_alterar_descricao_invalida(string novaDescricao, string mensagemErro)
    {
        var aula = CriarInstanciaAula();

        Action act = () => aula.AlterarDescricao(novaDescricao);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_alterar_descricao_valida()
    {
        // Arrange
        var novadescricao = "Introdução ao Domain Driven Design Atualizado";
        var aula = CriarInstanciaAula();

        // Act
        aula.AlterarDescricao(novadescricao);

        // Assert
        aula.Descricao.Should().Be(novadescricao);
    }

    [Theory]
    [InlineData(0, "*Carga horária deve ser maior que zero*")]
    [InlineData(6, "*Carga horária deve estar entre 1 e 5 horas*")]
    public void Nao_deve_alterar_carga_horaria_invalida(short novaCarga, string mensagemErro)
    {
        var aula = CriarInstanciaAula();

        Action act = () => aula.AlterarCargaHoraria(novaCarga);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_alterar_carga_horaria_valida()
    {
        // Arrange
        short novaCargaHoraria = 5;
        var aula = CriarInstanciaAula();

        // Act
        aula.AlterarCargaHoraria(novaCargaHoraria);

        // Assert
        aula.CargaHoraria.Should().Be(novaCargaHoraria);
    }

    [Theory]
    [InlineData(0, "*Ordem da aula deve ser maior que zero*")]
    public void Nao_deve_alterar_ordem_invalida(byte novaOrdem, string mensagemErro)
    {
        var aula = CriarInstanciaAula();

        Action act = () => aula.AlterarOrdemAula(novaOrdem);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_alterar_ordem_valida()
    {
        // Arrange
        byte novaOrdem = 10;
        var aula = CriarInstanciaAula();

        // Act
        aula.AlterarOrdemAula(novaOrdem);

        // Assert
        aula.OrdemAula.Should().Be(novaOrdem);
    }
    #endregion
}
