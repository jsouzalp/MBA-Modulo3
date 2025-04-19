using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Faturamento.Domain.ValueObjects;

namespace Plataforma.Educacao.Faturamento.Tests.Domains;
public class DadosCartaoTests
{
    #region Helpers
    private const string _numeroValido = "5493813493490144";
    private const string _nomeValido = "Jairo Azevedo";
    private const string _validadeValida = "12/96";
    private const string _cvvValido = "593";

    private static DadosCartao CriarCartao(string numero = _numeroValido,
                                           string nome = _nomeValido,
                                           string validade = _validadeValida,
                                           string cvv = _cvvValido)
    {
        return new DadosCartao(numero, nome, validade, cvv);
    }
    #endregion

    #region Construtores
    [Fact]
    public void Deve_criar_dados_cartao_validos()
    {
        var cartao = CriarCartao();

        cartao.Should().NotBeNull();
        cartao.Numero.Should().Be(_numeroValido);
        cartao.NomeTitular.Should().Be(_nomeValido);
        cartao.Validade.Should().Be(_validadeValida);
        cartao.CVV.Should().Be(_cvvValido);
    }

    [Theory]
    [InlineData(null, _nomeValido, _validadeValida, _cvvValido, "*Número do cartão deve ser informado*")]
    [InlineData("123", _nomeValido, _validadeValida, _cvvValido, "*Número do cartão deve possuir 16 caracteres*")]
    [InlineData("12345678901234567", _nomeValido, _validadeValida, _cvvValido, "*Número do cartão deve possuir 16 caracteres*")]
    [InlineData(_numeroValido, null, _validadeValida, _cvvValido, "*Nome do titular deve ser informado*")]
    [InlineData(_numeroValido, "ab", _validadeValida, _cvvValido, "*Nome do titular deve ter entre 3 e 50 caracteres*")]
    [InlineData(_numeroValido, _nomeValido, null, _cvvValido, "*Validade do cartão deve ser informada*")]
    [InlineData(_numeroValido, _nomeValido, "13/29", _cvvValido, "*Validade do cartão deve estar no formato MM/AA*")]
    [InlineData(_numeroValido, _nomeValido, _validadeValida, null, "*CVV deve ser informado*")]
    [InlineData(_numeroValido, _nomeValido, _validadeValida, "12", "*CVV deve possuir 3 caracteres*")]
    [InlineData(_numeroValido, _nomeValido, _validadeValida, "1283", "*CVV deve possuir 3 caracteres*")]
    [InlineData(_numeroValido, _nomeValido, _validadeValida, "abc", "*CVV deve conter apenas números*")]
    public void Nao_deve_criar_dados_cartao_invalidos(string numero, string nome, string validade, string cvv, string mensagemEsperada)
    {
        Action act = () => CriarCartao(numero ?? string.Empty,
            nome ?? string.Empty,
            validade ?? string.Empty,
            cvv ?? string.Empty);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemEsperada);
    }
    #endregion

    #region Metodos do Dominio
    //[Fact]
    //public void Deve_atualizar_numero_valido()
    //{
    //    var cartao = CriarCartao();
    //    var novoNumero = "8765432187654321";

    //    cartao.AtualizarNumero(novoNumero);

    //    cartao.Numero.Should().Be(novoNumero);
    //}

    //[Theory]
    //[InlineData(null, "*Número do cartão deve ser informado*")]
    //[InlineData("123", "*Número do cartão deve possuir 16 caracteres*")]
    //public void Nao_deve_atualizar_numero_invalido(string numeroInvalido, string mensagemErro)
    //{
    //    var cartao = CriarCartao();

    //    Action act = () => cartao.AtualizarNumero(numeroInvalido);

    //    act.Should().Throw<DomainException>()
    //       .WithMessage(mensagemErro);
    //}

    //[Fact]
    //public void Deve_atualizar_nome_valido()
    //{
    //    var cartao = CriarCartao();
    //    var novoNome = "Beltrano da Silva";

    //    cartao.AtualizarNomeTitular(novoNome);

    //    cartao.NomeTitular.Should().Be(novoNome);
    //}

    //[Theory]
    //[InlineData(null, "*Nome do titular deve ser informado*")]
    //[InlineData("ab", "*Nome do titular deve ter entre 3 e 50 caracteres*")]
    //public void Nao_deve_atualizar_nome_invalido(string nome, string mensagemErro)
    //{
    //    var cartao = CriarCartao();

    //    Action act = () => cartao.AtualizarNomeTitular(nome);

    //    act.Should().Throw<DomainException>()
    //       .WithMessage(mensagemErro);
    //}

    //[Fact]
    //public void Deve_atualizar_validade_valida()
    //{
    //    var cartao = CriarCartao();
    //    var novaValidade = "11/30";

    //    cartao.AtualizarValidade(novaValidade);

    //    cartao.Validade.Should().Be(novaValidade);
    //}

    //[Theory]
    //[InlineData(null, "*Validade do cartão deve ser informada*")]
    //[InlineData("13/29", "*Validade do cartão deve estar no formato MM/AA*")]
    //public void Nao_deve_atualizar_validade_invalida(string validade, string mensagemErro)
    //{
    //    var cartao = CriarCartao();

    //    Action act = () => cartao.AtualizarValidade(validade);

    //    act.Should().Throw<DomainException>()
    //       .WithMessage(mensagemErro);
    //}

    //[Fact]
    //public void Deve_atualizar_cvv_valido()
    //{
    //    var cartao = CriarCartao();
    //    var novoCvv = "999";

    //    cartao.AtualizarCVV(novoCvv);

    //    cartao.CVV.Should().Be(novoCvv);
    //}

    //[Theory]
    //[InlineData(null, "*CVV deve ser informado*")]
    //[InlineData("12", "*CVV deve possuir 3 caracteres*")]
    //[InlineData("1235", "*CVV deve possuir 3 caracteres*")]
    //[InlineData("abc", "*CVV deve conter apenas números*")]
    //public void Nao_deve_atualizar_cvv_invalido(string cvv, string mensagemErro)
    //{
    //    var cartao = CriarCartao();

    //    Action act = () => cartao.AtualizarCVV(cvv);

    //    act.Should().Throw<DomainException>()
    //       .WithMessage(mensagemErro);
    //}
    #endregion
}
