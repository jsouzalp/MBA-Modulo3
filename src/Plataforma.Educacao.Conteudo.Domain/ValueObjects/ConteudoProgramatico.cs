using Plataforma.Educacao.Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Domain.ValueObjects
{
    public class ConteudoProgramatico
    {
        #region Atributos
        public string Finalidade { get; private set; }
        public string Ementa { get; private set; }
        #endregion

        #region Construtores
        public ConteudoProgramatico(string finalidade, string ementa)
        {
            Finalidade = finalidade;
            Ementa = ementa;

            ValidarIntegridadeConteudoProgramatico();
        }
        #endregion

        #region Setters
        public void AlterarFinalidade(string finalidade)
        {
            ValidarIntegridadeConteudoProgramatico(novaFinalidade: finalidade);
            Finalidade = finalidade;
        }

        public void AlterarEmenta(string ementa)
        {
            ValidarIntegridadeConteudoProgramatico(novaEmenta: ementa);
            Ementa = ementa;
        }
        #endregion

        #region Validações
        private void ValidarIntegridadeConteudoProgramatico(string novaFinalidade = null, string novaEmenta = null)
        {
            var finalidade = novaFinalidade ?? Finalidade;
            var ementa = novaEmenta ?? Ementa;

            var validacao = new ResultadoValidacao<ConteudoProgramatico>();
            ValidacaoTexto.DevePossuirConteudo(finalidade, "Finalidade não pode ser vazio ou nulo", validacao);
            ValidacaoTexto.DevePossuirTamanho(finalidade, "Finalidade do conteúdo programático deve ter entre 10 e 100 caracteres", 10, 100, validacao);
            ValidacaoTexto.DevePossuirConteudo(ementa, "Finalidade do conteúdo programático não pode ser vazio ou nulo", validacao);
            ValidacaoTexto.DevePossuirTamanho(ementa, "Finalidade do conteúdo programático deve ter entre 10 e 100 caracteres", 10, 100, validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"Finalidade: {Finalidade}";
        }
        #endregion
    }
}
