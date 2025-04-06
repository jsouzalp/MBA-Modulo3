using Plataforma.Educacao.Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Validations;
using System.Text.Json.Serialization;

namespace Plataforma.Educacao.Conteudo.Domain.Entities
{
    public class Curso : Entidade, IRaizAgregacao
    {
        #region Atributos

        public string Nome { get; private set; }
        public decimal Valor { get; private set; }
        public bool Ativo { get; private set; }
        //public DateTime DataCriacao { get; private set; }
        public DateTime? ValidoAte { get; private set; }

        #region Helper only for EF Mapping
        public ConteudoProgramatico ConteudoProgramatico { get; private set; }

        public ICollection<Aula> Aulas { get; private set; }

        #endregion

        #endregion

        #region Construtores
        // EF Constructor
        protected Curso() { }

        public Curso(string nome, decimal valor, DateTime? validoAte, ConteudoProgramatico conteudoProgramatico, ICollection<Aula> aulas = null)
        {
            Nome = nome;
            Valor = valor;
            ValidoAte = validoAte;
            ConteudoProgramatico = conteudoProgramatico;
            Aulas = aulas;
            //DataCriacao = DateTime.Now;

            ValidarIntegridadeCurso();
        }
        #endregion

        #region Getters
        #endregion

        #region Setters
        public void AtivarCurso() => Ativo = true;
        public void DesativarCurso() => Ativo = false;

        public void AlterarNome(string nome)
        {
            ValidarIntegridadeCurso(novoNome: nome);
            Nome = nome;
        }

        public void AlterarValor(decimal valor)
        {
            ValidarIntegridadeCurso(novoValor: valor);
            Valor = valor;
        }

        public void AlterarValidoAte(DateTime? validoAte)
        {
            ValidoAte = validoAte;
        }
        public void AlterarConteudoProgramatico(ConteudoProgramatico conteudoProgramatico)
        {
            ValidarIntegridadeCurso(novoConteudoProgramatico: conteudoProgramatico);
            ConteudoProgramatico = conteudoProgramatico;
        }

        public void AdicionarAula(Aula aula)
        {
            Aulas ??= [];
            Aulas.Add(aula);
        }

        public void RemoverAula(Aula aula)
        {
            if (Aulas == null) return;
            Aulas = Aulas.Where(a => a.Id != aula.Id).ToList();
        }

        public int CargaHoraria => Aulas?.Sum(a => a.CargaHoraria) ?? 0;
        public int QuantidadeAulas => Aulas?.Count() ?? 0;
        #endregion

        #region Validações
        private void ValidarIntegridadeCurso(string novoNome = null, decimal? novoValor = null, ConteudoProgramatico novoConteudoProgramatico = null)
        {
            var nome = novoNome ?? Nome;
            var valor = novoValor ?? Valor;
            var conteudoProgramatico = novoConteudoProgramatico ?? ConteudoProgramatico;

            var validacao = new ResultadoValidacao<Curso>();
            ValidacaoTexto.DevePossuirConteudo(nome, "Nome do curso não pode ser vazio ou nulo", validacao);
            ValidacaoTexto.DevePossuirTamanho(nome, 10, 100, "Nome do curso deve ter entre 10 e 100 caracteres", validacao);
            ValidacaoNumerica.DeveSerMaiorQueZero(valor, "Valor do curso deve ser maior que zero", validacao);
            ValidacaoObjeto.DeveEstarInstanciado(conteudoProgramatico, "Conteúdo programático não foi informado", validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            string cursoAtivo = Ativo ? "Sim" : "Não";
            return $"Curso {Nome} com valor de {Valor:N2} (Ativo? {cursoAtivo})";
        }
        #endregion
    }
}
