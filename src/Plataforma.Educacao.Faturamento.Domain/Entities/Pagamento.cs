using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.DomainValidations;
using Plataforma.Educacao.Faturamento.Domain.Enumerators;
using Plataforma.Educacao.Faturamento.Domain.ValueObjects;

namespace Plataforma.Educacao.Faturamento.Domain.Entities;
public class Pagamento : Entidade, IRaizAgregacao
{
    #region Atributos        
    public Guid MatriculaId { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    public DadosCartao Cartao { get; private set; }
    public StatusPagamento StatusPagamento { get; private set; }
    public string CodigoConfirmacaoPagamento { get; private set; }

    protected Pagamento() { }
    public Pagamento(Guid matriculaId, decimal valor, DateTime dataVencimento)
    {
        MatriculaId = matriculaId;
        Valor = valor;
        DataVencimento = dataVencimento;
        StatusPagamento = new StatusPagamento(StatusPagamentoEnum.Pendente);

        ValidarIntegridadePagamento();
    }
    #endregion

    #region Métodos
    public bool PossuiPagamentoAprovado() => StatusPagamento.EstahAprovado;
    //public bool PossuiPagamentoRecusado() => StatusPagamento.EstahRecusado;
    //public bool EstaVencido() => StatusPagamento.EstahPendente && DateTime.Now.Date > DataVencimento;
    //public bool PodeConfirmarPagamento() => StatusPagamento.EstahPendente && !DataPagamento.HasValue;

    public void ConfirmarPagamento(DateTime? dataPagamento, string codigoConfirmacaoPagamento, DadosCartao cartao)
    {
        dataPagamento ??= DateTime.Now;
        ValidarIntegridadePagamento(novaDataPagamento: dataPagamento, novoCodigoConfirmacaoPagamento: codigoConfirmacaoPagamento);

        StatusPagamento.TransicionarPara(StatusPagamentoEnum.Aprovado);
        DataPagamento = dataPagamento;
        Cartao = cartao;
    }

    public void RecusarPagamento()
    {
        StatusPagamento.TransicionarPara(StatusPagamentoEnum.Recusado);
        DataPagamento = null;
    }

    //public void AlterarDataVencimento(DateTime novaDataVencimento)
    //{
    //    ValidarIntegridadePagamento(novaDataVencimento: novaDataVencimento);
    //    DataVencimento = novaDataVencimento;
    //}

    //public void CorrigirDataPagamento(DateTime novaDataPagamento)
    //{
    //    ValidarIntegridadePagamento(novaDataPagamento: novaDataPagamento);
    //    DataPagamento = novaDataPagamento;
    //}

    private void ValidarIntegridadePagamento(DateTime? novaDataPagamento = null, string novoCodigoConfirmacaoPagamento = null)
    {
        var matriculaId = MatriculaId;
        var valor = Valor;
        var dataPagamento = novaDataPagamento ?? DataPagamento;
        var codigoConfirmacaoPagamento = novoCodigoConfirmacaoPagamento ?? CodigoConfirmacaoPagamento;

        var validacao = new ResultadoValidacao<Pagamento>();

        ValidacaoGuid.DeveSerValido(matriculaId, "Matrícula do curso não foi informada", validacao);
        ValidacaoNumerica.DeveSerMaiorQueZero(valor, "Valor do pagamento deve ser maior que zero", validacao);
        ValidacaoData.DeveSerValido(DataVencimento, "Data de vencimento deve ser válida", validacao);
        ValidacaoData.DeveSerMaiorQue(DataVencimento, DateTime.Now, "Data de vencimento deve ser futura", validacao);

        if (dataPagamento.HasValue)
        {
            ValidacaoTexto.DevePossuirConteudo(codigoConfirmacaoPagamento, "Código de confirmação do pagamento deve ser informado", validacao);
            ValidacaoData.DeveSerValido(dataPagamento.Value, "Data de pagamento deve ser válida", validacao);
            ValidacaoData.DeveSerMenorQue(dataPagamento.Value, DateTime.Now.AddDays(1), "Data de pagamento deve ser igual ou menor que a data atual", validacao);
        }

        if (!string.IsNullOrEmpty(codigoConfirmacaoPagamento))
        {
            ValidacaoTexto.DevePossuirTamanho(codigoConfirmacaoPagamento, 1, 100, "Código de confirmação do pagamento deve ter entre 1 e 100 caracteres", validacao);
        }

        validacao.DispararExcecaoDominioSeInvalido();
    }

    public override string ToString() => $"Pagamento de R${Valor:0.00}, vencimento: {DataVencimento:dd/MM/yyyy}, status: {StatusPagamento}";
    #endregion    
}
