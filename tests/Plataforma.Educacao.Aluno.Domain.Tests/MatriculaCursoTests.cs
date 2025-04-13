using FluentAssertions;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Enumerators;
using Plataforma.Educacao.Core.Exceptions;

namespace Plataforma.Educacao.Aluno.Domain.Tests
{
    public class MatriculaCursoTests
    {
        #region Helpers
        private const string _alunoIdValido = "11111111-1111-1111-1111-111111111111";
        private const string _cursoIdValido = "22222222-2222-2222-2222-222222222222";
        private const string _guidInvalido = "00000000-0000-0000-0000-000000000000";
        private const double _valorValido = 1000.00;
        private const double _valorInvalido = 0.00;

        private MatriculaCurso CriarMatricula(string alunoId = _alunoIdValido, string cursoId = _cursoIdValido, double? valor = _valorValido)
        {
            return new MatriculaCurso(Guid.Parse(alunoId), Guid.Parse(cursoId), (decimal?)valor ?? 0.00m);
        }
        #endregion

        #region Construtores
        [Fact]
        public void Deve_criar_matricula_valida()
        {
            var matricula = CriarMatricula();

            matricula.Should().NotBeNull();
            matricula.AlunoId.Should().Be(Guid.Parse(_alunoIdValido));
            matricula.CursoId.Should().Be(Guid.Parse(_cursoIdValido));
            matricula.Valor.Should().Be((decimal)_valorValido);
            matricula.EstadoMatricula.Should().Be(EstadoMatriculaCursoEnum.PendentePagamento);
            matricula.DataMatricula.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData(_guidInvalido, _cursoIdValido, _valorValido, "*Aluno deve ser informado*")]
        [InlineData(_alunoIdValido, _guidInvalido, _valorValido, "*Curso deve ser informado*")]
        [InlineData(_alunoIdValido, _guidInvalido, _valorInvalido, "*Valor da matrícula deve ser maior que zero*")]
        public void Nao_deve_criar_matricula_invalida(string alunoId, string cursoId, double valor, string mensagemErro)
        {
            Action act = () => CriarMatricula(alunoId, cursoId, valor);

            act.Should().Throw<DomainException>()
               .WithMessage(mensagemErro);
        }
        #endregion

        #region Setters
        [Fact]
        public void Estado_inicial_da_matricula_deve_ser_pendente_pagamento()
        {
            // Arrange & Act
            var matricula = CriarMatricula();

            // Assert
            matricula.EstadoMatricula.Should().Be(EstadoMatriculaCursoEnum.PendentePagamento);
        }

        [Fact]
        public void Deve_atualizar_pagamento_matricula()
        {
            var matricula = CriarMatricula();

            matricula.AtualizarPagamentoMatricula();

            matricula.EstadoMatricula.Should().Be(EstadoMatriculaCursoEnum.PagamentoRealizado);
        }

        [Fact]
        public void Deve_concluir_curso_quando_pagamento_realizado()
        {
            var matricula = CriarMatricula();
            matricula.AtualizarPagamentoMatricula();

            matricula.ConcluirCurso();

            matricula.CursoConcluido.Should().BeTrue();
            matricula.DataConclusao.Should().NotBeNull();
        }

        [Fact]
        public void Nao_deve_concluir_curso_quando_abandonado()
        {
            var matricula = CriarMatricula();
            matricula.AtualizarAbandonoMatricula();

            Action act = () => matricula.ConcluirCurso();

            act.Should().Throw<DomainException>()
               .WithMessage("*Não é possível concluir um curso com estado de pagamento abandonado*");
        }

        [Fact]
        public void Nao_deve_abandonar_curso_ja_concluido()
        {
            var matricula = CriarMatricula();
            matricula.AtualizarPagamentoMatricula();
            matricula.ConcluirCurso();

            Action act = () => matricula.AtualizarAbandonoMatricula();

            act.Should().Throw<DomainException>()
               .WithMessage("*Não é possível alterar o estado da matrícula para pagamento abandonado com o curso concluído*");
        }
        #endregion
    }
}