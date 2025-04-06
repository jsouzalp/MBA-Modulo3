using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Validations;
using System.Text.Json.Serialization;

namespace Plataforma.Educacao.Aluno.Domain.Entities
{
    public class Aluno : Entidade, IRaizAgregacao
    {
        #region Atributos
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public DateTime DataNascimento { get; private set; }

        #region Helper only for EF Mapping
        public ICollection<MatriculaCurso> MatriculasCursos { get; private set; }
        #endregion
        #endregion

        #region Construtores
        protected Aluno() { }

        public Aluno(string nome, string email, DateTime dataNascimento)
        {
            Nome = nome;
            Email = email;
            DataNascimento = dataNascimento;

            ValidarIntegridadeAluno();
        }
        #endregion

        #region Setters
        public void AtualizarNome(string nome)
        {
            ValidarIntegridadeAluno(novoNome: nome ?? string.Empty);
            Nome = nome;
        }

        public void AtualizarEmail(string email)
        {
            ValidarIntegridadeAluno(novoEmail: email ?? string.Empty);
            Email = email;
        }

        public void AtualizarDataNascimento(DateTime dataNascimento)
        {
            ValidarIntegridadeAluno(novaDataNascimento: dataNascimento);
            DataNascimento = dataNascimento;
        }

        #endregion

        #region Validações
        private void ValidarIntegridadeAluno(string novoNome = null, string novoEmail = null, DateTime? novaDataNascimento = null)
        {
            var nome = novoNome ?? Nome;
            var email = novoEmail ?? Email;
            var dataNascimento = novaDataNascimento ?? DataNascimento;

            var validacao = new ResultadoValidacao<Aluno>();

            ValidacaoTexto.DevePossuirConteudo(nome, "Nome não pode ser nulo ou vazio", validacao);
            ValidacaoTexto.DevePossuirTamanho(nome, 3, 50, "Nome deve ter entre 3 e 50 caracteres", validacao);
            ValidacaoTexto.DevePossuirConteudo(email, "Email não pode ser nulo ou vazio", validacao);
            ValidacaoTexto.DevePossuirTamanho(email, 3, 100, "Email deve ter entre 3 e 100 caracteres", validacao);
            ValidacaoTexto.DeveAtenderRegex(email, @"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,}$", "Email informado é inválido", validacao);
            ValidacaoData.DeveSerValido(dataNascimento, "Data de nascimento deve ser válida", validacao);
            ValidacaoData.DeveSerMenorQue(dataNascimento, DateTime.Now, "Data de nascimento não pode ser superior à data atual", validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"{Nome} (Email: {Email})";
        }
        #endregion
    }
}
