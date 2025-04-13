using Plataforma.Educacao.Core.Validations;
using Plataforma.Educacao.Faturamento.Domain.Enumerators;

namespace Plataforma.Educacao.Faturamento.Domain.ValueObjects
{
    public class StatusPagamento
    {
        #region Atributos
        public StatusPagamentoEnum Status { get; private set; }
        #endregion

        #region Construtores
        public StatusPagamento(StatusPagamentoEnum status)
        {
            Status = status;
        }
        #endregion

        #region Getters
        public bool EstahPendente => Status == StatusPagamentoEnum.Pendente;
        public bool EstahAprovado => Status == StatusPagamentoEnum.Aprovado;
        public bool EstahRecusado => Status == StatusPagamentoEnum.Recusado;
        #endregion

        #region Metodos do Dominio
        public void TransicionarPara(StatusPagamentoEnum novoStatus)
        {
            ValidarIntegridadeStatusPagamento(novoStatus);
            Status = novoStatus;
        }
        #endregion

        #region Validação
        private void ValidarIntegridadeStatusPagamento(StatusPagamentoEnum status)
        {
            var validacao = new ResultadoValidacao<StatusPagamento>();

            if (Status == StatusPagamentoEnum.Aprovado) { validacao.AdicionarErro("Pagamento aprovado não pode ser alterado."); }
            if (Status == StatusPagamentoEnum.Recusado) { validacao.AdicionarErro("Pagamento recusado não pode ser alterado."); }
            if (Status == StatusPagamentoEnum.Pendente && status == StatusPagamentoEnum.Pendente) { validacao.AdicionarErro("Pagamento já está pendente."); }

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        public override string ToString() => Status.ToString();
        #endregion
    }
}
