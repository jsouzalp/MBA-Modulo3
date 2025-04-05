using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Domain.Entities
{
    public class Aula : Entidade
    {
        #region Atributos
        public Guid CursoId { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public int CargaHoraria { get; private set; }
        public byte OrdemAula { get; private set; }
        #endregion

        #region Construtores
        protected Aula() { }

        public Aula(Guid cursoId, string descricao, int cargaHoraria, byte ordemAula)
        {
            CursoId = cursoId;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
            OrdemAula = ordemAula;

            ValidarIntegridadeAula();
        }
        #endregion

        #region Setters
        public void AtivarAula() => Ativo = true;

        public void DeativarAula() => Ativo = false;

        public void AlterarCurso(Guid cursoId)
        {
            ValidarIntegridadeAula(novoCursoId: cursoId);
            CursoId = cursoId;
        }

        public void AlterarDescricao(string descricao)
        {
            ValidarIntegridadeAula(novaDescricao: descricao);
            Descricao = descricao;
        }

        public void AlterarCargaHoraria(int cargaHoraria)
        {
            ValidarIntegridadeAula(novaCargaHoraria: cargaHoraria);
            CargaHoraria = cargaHoraria;
        }

        public void AlterarOrdemAula(byte ordemAula)
        {
            ValidarIntegridadeAula(novaOrdemAula: ordemAula);
            OrdemAula = ordemAula;
        }
        #endregion

        #region Validacoes
        private void ValidarIntegridadeAula(Guid? novoCursoId = null, string novaDescricao = null, int? novaCargaHoraria = null, byte? novaOrdemAula = null)
        {
            var cursoId = novoCursoId ?? CursoId;
            var descricao = novaDescricao ?? Descricao;
            var cargaHoraria = novaCargaHoraria ?? CargaHoraria;
            var ordemAula = novaOrdemAula ?? OrdemAula;

            var validacao = new ResultadoValidacao<Aula>();
            ValidacaoGuid.DeveSerValido(cursoId, "Id do curso não pode ser vazio", validacao);
            ValidacaoTexto.DevePossuirConteudo(descricao, "Descrição da aula não pode ser vazia ou nula", validacao);
            ValidacaoTexto.DevePossuirTamanho(descricao, "Descrição da aula deve ter entre 5 e 100 caracteres", 5, 100, validacao);
            ValidacaoNumerica.DeveSerMaiorQueZero(cargaHoraria, "Carga horária deve ser maior que zero", validacao);
            ValidacaoNumerica.DeveEstarEntre(cargaHoraria, 1, 5, "Carga horária deve estar entre 1 e 5 horas", validacao);
            ValidacaoNumerica.DeveSerMaiorQueZero(ordemAula, "Ordem da aula deve ser maior que zero", validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        private string _aulaAtiva => Ativo ? "Sim" : "Não";
        public override string ToString()
        {
            return $"Aula {Descricao} com carga horária de {CargaHoraria} e ordem {OrdemAula} (Ativo? {_aulaAtiva})";
        }
        #endregion
    }
}
