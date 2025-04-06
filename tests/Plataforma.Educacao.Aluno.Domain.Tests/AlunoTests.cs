using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Domain.Tests
{
    public class AlunoTests
    {
        #region Helpers
        private const string _nomeValido = "Jairo Azevedo";
        private const string _emailValido = "jsouza.lp@gmail.com";
        private static readonly DateTime _dataNascimentoValida = new DateTime(1973, 06, 25);

        private Entities.Aluno CriarInstanciaAluno(string nome = _nomeValido, string email = _emailValido, DateTime? dataNascimento = null)
        {
            return new Entities.Aluno(nome, email, dataNascimento ?? _dataNascimentoValida);
        }
        #endregion

        #region Construtores
        [Fact]
        public void Deve_criar_aluno_valido()
        {
            // Act
            var aluno = CriarInstanciaAluno();

            // Assert
            aluno.Nome.Should().Be(_nomeValido);
            aluno.Email.Should().Be(_emailValido);
            aluno.DataNascimento.Should().Be(_dataNascimentoValida);
        }

        [Theory]
        [InlineData("", "joao@email.com", "2000-01-01", "*Nome não pode ser nulo ou vazio*")]
        [InlineData("Jo", "joao@email.com", "2000-01-01", "*Nome deve ter entre 3 e 50 caracteres*")]
        [InlineData("João", "", "2000-01-01", "*Email não pode ser nulo ou vazio*")]
        [InlineData("João", "a@b", "2000-01-01", "*Email informado é inválido*")]
        [InlineData("João", "joao@email.com", "0001-01-01", "*Data de nascimento deve ser válida*")]
        [InlineData("João", "joao@email.com", "2099-01-01", "*Data de nascimento não pode ser superior à data atual*")]
        public void Nao_deve_criar_aluno_invalido(string nome, string email, string dataNascimentoStr, string mensagemErro)
        {
            var dataNascimento = DateTime.Parse(dataNascimentoStr);

            Action act = () => CriarInstanciaAluno(nome, email, dataNascimento);

            act.Should().Throw<DomainException>()
               .WithMessage(mensagemErro);
        }
        #endregion

        #region Setters
        [Fact]
        public void Deve_atualizar_nome_valido()
        {
            var aluno = CriarInstanciaAluno();
            var novoNome = "Maria Oliveira";

            aluno.AtualizarNome(novoNome);

            aluno.Nome.Should().Be(novoNome);
        }

        //ValidacaoTexto.DevePossuirConteudo(nome, "Nome não pode ser nulo ou vazio", validacao);
        //    ValidacaoTexto.DevePossuirTamanho(nome, 3, 50, "Nome deve ter entre 3 e 50 caracteres", validacao);
        //    ValidacaoTexto.DevePossuirConteudo(email, "Email não pode ser nulo ou vazio", validacao);
        //    ValidacaoTexto.DevePossuirTamanho(email, 3, 100, "Email deve ter entre 3 e 100 caracteres", validacao);
        //    ValidacaoTexto.DeveAtenderRegex(email, @"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,}$", "Email informado é inválido", validacao);
        //    ValidacaoData.DeveSerValido(dataNascimento, "Data de nascimento deve ser válida", validacao);
        //    ValidacaoData.DeveSerMenorQue(dataNascimento, DateTime.Now, "Data de nascimento não pode ser superior à data atual", validacao);



        [Theory]
        [InlineData("", "*Nome não pode ser nulo ou vazio*")]
        [InlineData("Jo", "*Nome deve ter entre 3 e 50 caracteres*")]
        public void Nao_deve_atualizar_nome_invalido(string novoNome, string mensagemErro)
        {
            var aluno = CriarInstanciaAluno();

            Action act = () => aluno.AtualizarNome(novoNome);

            act.Should().Throw<DomainException>()
               .WithMessage(mensagemErro);
        }

        [Fact]
        public void Deve_atualizar_email_valido()
        {
            var aluno = CriarInstanciaAluno();
            var novoEmail = "novo@email.com";

            aluno.AtualizarEmail(novoEmail);

            aluno.Email.Should().Be(novoEmail);
        }

        [Theory]
        [InlineData("", "*Email não pode ser nulo ou vazio*")]
        [InlineData("x@x", "*Email informado é inválido*")]
        public void Nao_deve_atualizar_email_invalido(string novoEmail, string mensagemErro)
        {
            var aluno = CriarInstanciaAluno();

            Action act = () => aluno.AtualizarEmail(novoEmail);

            act.Should().Throw<DomainException>()
               .WithMessage(mensagemErro);
        }

        [Fact]
        public void Deve_atualizar_data_nascimento_valida()
        {
            var aluno = CriarInstanciaAluno();
            var novaData = new DateTime(1995, 5, 10);

            aluno.AtualizarDataNascimento(novaData);

            aluno.DataNascimento.Should().Be(novaData);
        }

        [Fact]
        public void Nao_deve_atualizar_data_nascimento_futura()
        {
            var aluno = CriarInstanciaAluno();
            var dataFutura = DateTime.Now.AddDays(1);

            Action act = () => aluno.AtualizarDataNascimento(dataFutura);

            act.Should().Throw<DomainException>()
               .WithMessage("*Data de nascimento não pode ser superior à data atual*");
        }
        #endregion
    }
}