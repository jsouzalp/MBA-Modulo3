using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Domain.ValueObjects
{
    public class HistoricoAprendizado
    {
        #region Atributos
        public Guid AulaId { get; private set; }
        public string NomeAula { get; private set; }
        public DateTime DataConclusao { get; private set; }
        #endregion

        #region Construtores
        protected HistoricoAprendizado() { }
        public HistoricoAprendizado(Guid aulaId, string nomeAula)
        {
            AulaId = aulaId;
            NomeAula = nomeAula;
            DataConclusao = DateTime.Now;

            ValidarIntegridadeHistoricoAprendizado();
        }
        #endregion

        #region Getters
        #endregion

        #region Setters 
        #endregion

        #region Validacoes 
        public void ValidarIntegridadeHistoricoAprendizado()
        {
            var validacao = new ResultadoValidacao<Certificado>();

            ValidacaoGuid.DeveSerValido(AulaId, "Identifição da aula não pode ser vazio", validacao);
            ValidacaoTexto.DevePossuirConteudo(NomeAula, "Nome da aula não pode ser vazio", validacao);
            ValidacaoTexto.DevePossuirTamanho(NomeAula, 5, 100, "Nome da aula deve ter entre 5 e 100 caracteres", validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        public override string ToString() => $"Aula {NomeAula} finalizada em {DataConclusao:dd/MM/yyyy}";
        #endregion

    }
}
