using FluentAssertions;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Core.Exceptions;

namespace Plataforma.Educacao.Conteudo.Domain.Tests;
public class CursoTests
{
    #region Helpers
    private const string _nomeValido = "Curso completo de Domain Driven Design";
    private const decimal _valorValido = 1000m;

    private static ConteudoProgramatico _conteudoValido => new("Especialização em DDD", "Conceitos básicos, intermediários e avançados de Domain Driven Design, CQRS e Event Sourcing");
    private static Aula _aulaValida1 => new(Guid.NewGuid(), "Aula introdutória", 1, 1, "http://google.com");
    private static Aula _aulaValida2 => new (Guid.NewGuid(), "Conceitos básicos", 3, 2, "http://google.com");
    private static Aula _aulaValida3 => new (Guid.NewGuid(), "Conceitos Conceitos avançados", 4, 3, "http://stackoverflow.com");

    private static Curso CriarCurso(string nome = _nomeValido,
        decimal valor = _valorValido,
        DateTime? validoAte = null,
        ConteudoProgramatico conteudo = null,
        ICollection<Aula> aulas = null)
    {
        return new Curso(nome,
            valor,
            validoAte,
            conteudo ?? _conteudoValido);
            //aulas ?? [_aulaValida1, _aulaValida2, _aulaValida3]);
    }
    #endregion

    #region Construtores
    [Fact]
    public void Deve_criar_curso_valido()
    {
        var curso = CriarCurso();

        curso.Should().NotBeNull();
        curso.Nome.Should().Be(_nomeValido);
        curso.Valor.Should().Be(_valorValido);
        curso.ConteudoProgramatico.Should().NotBeNull();
        //curso.CargaHoraria().Should().Be(8);
        //curso.QuantidadeAulas().Should().Be(3);
    }

    [Theory]
    [InlineData("", 100, "*Nome do curso não pode ser vazio ou nulo*")]
    [InlineData("abc", 100, "*Nome do curso deve ter entre 10 e 100 caracteres*")]
    [InlineData(_nomeValido, 0, "*Valor do curso deve ser maior que zero*")]
    public void Nao_deve_criar_curso_invalido(string nomeCurso, decimal valorCurso, string mensagemErro)
    {
        Action act = () => CriarCurso(nome: nomeCurso, valor: valorCurso);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }
    #endregion

    #region Metodos do Dominio
    [Fact]
    public void Deve_ativar_e_desativar_curso()
    {
        var curso = CriarCurso();

        curso.AtivarCurso();
        curso.Ativo.Should().BeTrue();

        curso.DesativarCurso();
        curso.Ativo.Should().BeFalse();
    }
    
    [Theory]
    [InlineData("", "*Nome do curso não pode ser vazio ou nulo*")]
    [InlineData("abc", "*Nome do curso deve ter entre 10 e 100 caracteres*")]
    public void Nao_deve_alterar_nome_invalido(string novoNome, string mensagemErro)
    {
        var curso = CriarCurso();

        Action act = () => curso.AlterarNome(novoNome);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_alterar_nome_valido()
    {
        var curso = CriarCurso();
        var novoNome = "Curso completo de Domain Driven Design atualizado";

        curso.AlterarNome(novoNome);

        curso.Nome.Should().Be(novoNome);
    }

    [Theory]
    [InlineData(0, "*Valor do curso deve ser maior que zero*")]
    public void Nao_deve_alterar_valor_invalido(decimal novoValor, string mensagemErro)
    {
        var curso = CriarCurso();

        Action act = () => curso.AlterarValor(novoValor);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_alterar_valor_valido()
    {
        var curso = CriarCurso();
        var novoValor = 1500m;

        curso.AlterarValor(novoValor);

        curso.Valor.Should().Be(novoValor);
    }

    [Fact]
    public void Deve_alterar_validade()
    {
        var curso = CriarCurso();
        var novaData = DateTime.Now.AddMonths(3);

        curso.AlterarValidadeCurso(novaData);

        curso.ValidoAte.Should().Be(novaData);
    }

    [Fact]
    public void Deve_alterar_conteudo_programatico()
    {
        var curso = CriarCurso();
        var novoConteudo = new ConteudoProgramatico("Aprender DDD", new string('A', 60));

        //curso.AlterarConteudoProgramatico(novoConteudo);
        curso.AtualizarConteudoProgramatico("Aprender DDD", new string('A', 60));

        curso.ConteudoProgramatico.Should().Be(novoConteudo);
    }
    #endregion

    #region Aulas
    [Fact]
    public void Deve_adicionar_e_remover_aula()
    {
        Aula aula1 = _aulaValida1;
        Aula aula2 = _aulaValida2;
        Aula aula3 = _aulaValida3;
        var curso = CriarCurso();
        curso.AdicionarAula(aula1.Descricao, aula1.CargaHoraria, aula1.OrdemAula, aula1.Url);
        curso.AdicionarAula(aula2.Descricao, aula2.CargaHoraria, aula2.OrdemAula, aula2.Url);
        Aula aulaParaRemover = curso.Aulas.Last();

        curso.QuantidadeAulas().Should().Be(2);
        curso.CargaHoraria().Should().Be(4);

        curso.AdicionarAula(aula3.Descricao, aula3.CargaHoraria, aula3.OrdemAula, aula3.Url);
        curso.QuantidadeAulas().Should().Be(3);
        curso.CargaHoraria().Should().Be(8);

        curso.RemoverAula(aulaParaRemover);
        curso.QuantidadeAulas().Should().Be(2);
        curso.CargaHoraria().Should().Be(5);
    }

    [Fact]
    public void Deve_retornar_curso_disponivel_quando_ativo_e_sem_validade()
    {
        var curso = CriarCurso();
        curso.AtivarCurso();

        curso.CursoDisponivel().Should().BeTrue();
    }

    [Fact]
    public void Deve_retornar_curso_disponivel_se_valido_ate_futuro()
    {
        var curso = CriarCurso(validoAte: DateTime.Now.AddDays(10));
        curso.AtivarCurso();

        curso.CursoDisponivel().Should().BeTrue();
    }

    [Fact]
    public void Nao_deve_retornar_curso_disponivel_quando_desativado()
    {
        var curso = CriarCurso(); 

        curso.CursoDisponivel().Should().BeFalse();
    }

    [Fact]
    public void Nao_deve_retornar_curso_disponivel_se_data_validade_passada()
    {
        var curso = CriarCurso(validoAte: DateTime.Now.AddDays(-10));
        curso.AtivarCurso();

        curso.CursoDisponivel().Should().BeFalse();
    }

    [Fact]
    public void Nao_deve_adicionar_aula_com_ordem_duplicada()
    {
        Aula aula1 = _aulaValida1;
        var curso = CriarCurso();
        curso.AdicionarAula(aula1.Descricao, aula1.CargaHoraria, aula1.OrdemAula, aula1.Url);
        var ordemDuplicada = curso.Aulas.First().OrdemAula;

        Action act = () => curso.AdicionarAula("Aula duplicada", 2, ordemDuplicada, "http://google.com");

        act.Should().Throw<DomainException>()
           .WithMessage("*Ordem da aula deve ser única dentro do curso*");
    }
    #endregion

    #region Overrides
    [Fact]
    public void ToString_deve_retornar_nome_valor_e_status()
    {
        var curso = CriarCurso();
        curso.AtivarCurso();

        var resultado = curso.ToString();

        resultado.Should().Contain(_nomeValido);
        resultado.Should().Contain(_valorValido.ToString("N2"));
        resultado.Should().Contain("Sim");
    }
    #endregion
}
