using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using Plataforma.Educacao.Faturamento.Domain.Enumerators;
using Plataforma.Educacao.Faturamento.Domain.ValueObjects;

namespace Plataforma.Educacao.Faturamento.Tests.Domains;
public class PagamentoTests
{
    #region Helpers
    private const string _matriculaIdValido = "11111111-1111-1111-1111-111111111111";
    private static readonly DateTime _dataVencimentoFutura = DateTime.Now.AddDays(7);
    private const double _valorValido = 1000.00;

    private const string _numeroValido = "5493813493490144";
    private const string _nomeValido = "Jairo Azevedo";
    private const string _validadeValida = "12/96";
    private const string _cvvValido = "593";

    private static DadosCartao _cartaoValido => new(numero: _numeroValido,
        nomeTitular: _nomeValido,
        validade: _validadeValida,
        cvv: _cvvValido);

    private static Pagamento CriarPagamento(string matriculaId = _matriculaIdValido,
        double valor = _valorValido,
        DateTime? vencimento = null,
        DadosCartao cartao = null)
    {
        return new Pagamento(Guid.Parse(matriculaId), (decimal)valor, vencimento ?? _dataVencimentoFutura);
    }
    #endregion

    #region Construtores
    [Fact]
    public void Deve_criar_pagamento_valido()
    {
        var pagamento = CriarPagamento();

        pagamento.Should().NotBeNull();
        pagamento.MatriculaId.Should().Be(Guid.Parse(_matriculaIdValido));
        pagamento.Valor.Should().Be((decimal)_valorValido);
        pagamento.DataVencimento.Date.Should().Be(_dataVencimentoFutura.Date);
        pagamento.Cartao.Should().NotBeNull();
        pagamento.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Pendente);
        pagamento.DataPagamento.Should().BeNull();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", _valorValido, "*Matrícula do curso não foi informada*")]
    [InlineData(_matriculaIdValido, 0.0, "*Valor do pagamento deve ser maior que zero*")]
    public void Nao_deve_criar_pagamento_invalido(string matriculaId, double valor, string mensagemEsperada)
    {
        Action act = () => CriarPagamento(matriculaId, valor);

        act.Should().Throw<DomainException>()
           .WithMessage(mensagemEsperada);
    }
    #endregion

    #region Métodos de Pagamento
    [Fact]
    public void Deve_confirmar_pagamento()
    {
        var pagamento = CriarPagamento();

        DadosCartao dadosCartao = new DadosCartao(_numeroValido, _nomeValido, _validadeValida, _cvvValido);
        pagamento.ConfirmarPagamento(null, null, dadosCartao);

        pagamento.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Aprovado);
        pagamento.DataPagamento.Should().NotBeNull();
        pagamento.PossuiPagamentoAprovado().Should().BeTrue();
    }

    [Fact]
    public void Deve_recusar_pagamento()
    {
        var pagamento = CriarPagamento();

        pagamento.RecusarPagamento();

        pagamento.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Recusado);
        pagamento.DataPagamento.Should().BeNull();
        //pagamento.PossuiPagamentoRecusado().Should().BeTrue();
    }
    #endregion

    #region Alterações de Datas
    //[Fact]
    //public void Deve_permitir_confirmar_pagamento()
    //{
    //    var pagamento = CriarPagamento();
    //    pagamento.PodeConfirmarPagamento().Should().Be(true);
    //}

    //[Fact]
    //public void Deve_alterar_data_vencimento_valida()
    //{
    //    var pagamento = CriarPagamento();
    //    var novaData = DateTime.Now.AddDays(10);

    //    pagamento.AlterarDataVencimento(novaData);

    //    pagamento.DataVencimento.Should().Be(novaData);
    //}

    //[Fact]
    //public void Deve_corrigir_data_pagamento_valida()
    //{
    //    var dataCorreta = DateTime.Now.AddDays(-2);

    //    var pagamento = CriarPagamento();
    //    pagamento.ConfirmarPagamento(null);

    //    pagamento.CorrigirDataPagamento(dataCorreta);

    //    pagamento.DataPagamento.Should().Be(dataCorreta);
    //}

    //[Fact]
    //public void Nao_deve_permitir_confirmar_pagamento()
    //{
    //    var pagamento = CriarPagamento();
    //    pagamento.ConfirmarPagamento(null);

    //    pagamento.PodeConfirmarPagamento().Should().Be(false);
    //}

    //[Theory]
    //[InlineData("2000-01-01", "*Data de vencimento deve ser futura*")]
    //[InlineData("0001-01-01", "*Data de vencimento deve ser válida*")]
    //public void Nao_deve_alterar_data_vencimento_invalida(string dataTexto, string mensagemEsperada)
    //{
    //    var pagamento = CriarPagamento();
    //    var novaData = DateTime.Parse(dataTexto);

    //    Action act = () => pagamento.AlterarDataVencimento(novaData);

    //    act.Should().Throw<DomainException>()
    //       .WithMessage(mensagemEsperada);
    //}

    //[Theory]
    //[InlineData("2100-01-01", "*Data de pagamento deve ser igual ou menor que a data atual*")]
    //[InlineData("0001-01-01", "*Data de pagamento deve ser válida*")]
    //public void Nao_deve_corrigir_data_pagamento_invalida(string dataTexto, string mensagemEsperada)
    //{
    //    var pagamento = CriarPagamento();
    //    var dataFutura = DateTime.Parse(dataTexto);

    //    Action act = () => pagamento.CorrigirDataPagamento(dataFutura);

    //    act.Should().Throw<DomainException>()
    //       .WithMessage(mensagemEsperada);
    //}
    #endregion
}
