using Plataforma.Educacao.Core.Validations;

namespace Plataforma.Educacao.Faturamento.Domain.ValueObjects
{
    public class DadosCartao
    {
        #region Atributods
        public string Numero { get; private set; }
        public string NomeTitular { get; private set; }
        public string Validade { get; private set; }
        public string CVV { get; private set; }
        #endregion

        #region Construtores    
        public DadosCartao(string numero, string nomeTitular, string validade, string cvv)
        {
            Numero = numero;
            NomeTitular = nomeTitular;
            Validade = validade;
            CVV = cvv;

            ValidarIntegridadeDadosCartao();
        }
        #endregion

        #region Getters
        #endregion

        #region Setters
        public void AtualizarNumero(string numero)
        {
            ValidarIntegridadeDadosCartao(novoNumero: numero ?? string.Empty);
            Numero = numero;
        }

        public void AtualizarNomeTitular(string nomeTitular)
        {
            ValidarIntegridadeDadosCartao(novoNomeTitular: nomeTitular ?? string.Empty);
            NomeTitular = nomeTitular;
        }

        public void AtualizarValidade(string validade)
        {
            ValidarIntegridadeDadosCartao(novaValidade: validade ?? string.Empty);
            Validade = validade;
        }

        public void AtualizarCVV(string cvv)
        {
            ValidarIntegridadeDadosCartao(novoCvv: cvv ?? string.Empty);
            CVV = cvv;
        }
        #endregion

        #region Validações
        private void ValidarIntegridadeDadosCartao(string novoNumero = null, string novoNomeTitular = null, string novaValidade = null, string novoCvv = null)
        {
            string numero = novoNumero ?? Numero;
            string nomeTitular = novoNomeTitular ?? NomeTitular;
            string validade = novaValidade ?? Validade;
            string cvv = novoCvv ?? CVV;

            var validacao = new ResultadoValidacao<DadosCartao>();
            ValidacaoTexto.DevePossuirConteudo(numero, "Número do cartão deve ser informado", validacao);
            ValidacaoTexto.DevePossuirTamanho(numero, 16, 16, "Número do cartão deve possuir 16 caracteres", validacao);
            ValidacaoTexto.DevePossuirConteudo(nomeTitular, "Nome do titular deve ser informado", validacao);
            ValidacaoTexto.DevePossuirTamanho(nomeTitular, 3, 50, "Nome do titular deve ter entre 3 e 50 caracteres", validacao);
            ValidacaoTexto.DevePossuirConteudo(validade, "Validade do cartão deve ser informada", validacao);
            ValidacaoTexto.DeveAtenderRegex(validade, @"^(0[1-9]|1[0-2])\/\d{2}$", "Validade do cartão deve estar no formato MM/AA", validacao);
            ValidacaoTexto.DevePossuirConteudo(cvv, "CVV deve ser informado", validacao);
            ValidacaoTexto.DevePossuirTamanho(cvv, 3, 3, "CVV deve possuir 3 caracteres", validacao);
            ValidacaoTexto.DeveAtenderRegex(cvv, @"^\d{3}$", "CVV deve conter apenas números", validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        override public string ToString() => $"Dados do cartão: {NomeTitular}, validade: {Validade}";
        #endregion
    }
}
