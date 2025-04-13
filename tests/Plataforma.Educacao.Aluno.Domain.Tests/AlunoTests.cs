using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Plataforma.Educacao.Aluno.Domain.Tests;
public class AlunoTests
{
    #region Helpers
    private const string _nomeValido = "Jairo Azevedo";
    private const string _emailValido = "jsouza.lp@gmail.com";
    private static readonly DateTime _dataNascimentoValida = new(1973, 06, 25);

    private Entities.Aluno CriarAlunoValido(string nome = _nomeValido, string email = _emailValido, DateTime? dataNascimento = null)
    {
        return new Entities.Aluno(nome, email, dataNascimento ?? _dataNascimentoValida);
    }
    #endregion

    #region Construtores
    [Fact]
    public void Deve_criar_aluno_valido()
    {
        // Act
        var aluno = CriarAlunoValido();

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
        // Arrange
        var dataNascimento = DateTime.Parse(dataNascimentoStr);

        // Act
        Action act = () => CriarAlunoValido(nome, email, dataNascimento);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(mensagemErro);
    }

    [Fact]
    public void Nao_deve_criar_email_com_tamanho_maior_que_limite()
    {
        // Arrange
        var emailGrande = new string('a', 101) + "@email.com";

        // Act
        Action act = () => CriarAlunoValido("Fulano", emailGrande, DateTime.Now.AddYears(-20));

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Email deve ter entre 3 e 100 caracteres*");
    }
    #endregion

    #region Métodos do Domínio
    [Fact]
    public void Deve_atualizar_nome_valido()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var novoNome = "Maria Oliveira";

        // Act
        aluno.AtualizarNome(novoNome);

        // Assert
        aluno.Nome.Should().Be(novoNome);
    }

    [Theory]
    [InlineData("", "*Nome não pode ser nulo ou vazio*")]
    [InlineData("Jo", "*Nome deve ter entre 3 e 50 caracteres*")]
    public void Nao_deve_atualizar_nome_invalido(string novoNome, string mensagemErro)
    {
        // Arrange
        var aluno = CriarAlunoValido();

        // Act
        Action act = () => aluno.AtualizarNome(novoNome);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_atualizar_email_valido()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var novoEmail = "novo@email.com";

        // Act
        aluno.AtualizarEmail(novoEmail);

        // Assert
        aluno.Email.Should().Be(novoEmail);
    }

    [Theory]
    [InlineData("", "*Email não pode ser nulo ou vazio*")]
    [InlineData("x@x", "*Email informado é inválido*")]
    public void Nao_deve_atualizar_email_invalido(string novoEmail, string mensagemErro)
    {
        // Arrange
        var aluno = CriarAlunoValido();

        // Act
        Action act = () => aluno.AtualizarEmail(novoEmail);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(mensagemErro);
    }

    [Fact]
    public void Deve_atualizar_data_nascimento_valida()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var novaData = new DateTime(1995, 5, 10);

        // Act
        aluno.AtualizarDataNascimento(novaData);

        // Assert
        aluno.DataNascimento.Should().Be(novaData);
    }

    [Fact]
    public void Nao_deve_atualizar_data_nascimento_futura()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var dataFutura = DateTime.Now.AddDays(1);

        // Act
        Action act = () => aluno.AtualizarDataNascimento(dataFutura);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Data de nascimento não pode ser superior à data atual*");
    }

    [Fact]
    public void Deve_matricular_em_curso()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var cursoId = Guid.NewGuid();
        var nomeCurso = "Curso de DDD";
        var valor = 900m;

        // Act
        aluno.MatricularEmCurso(cursoId, nomeCurso, valor);

        // Assert
        aluno.MatriculasCursos.Should().ContainSingle();
        var matricula = aluno.MatriculasCursos.First();
        matricula.CursoId.Should().Be(cursoId);
        matricula.NomeCurso.Should().Be(nomeCurso);
        matricula.Valor.Should().Be(valor);
    }

    [Fact]
    public void Nao_deve_matricular_em_curso_com_valor_invalido()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var cursoId = Guid.NewGuid();

        // Act
        Action act = () => aluno.MatricularEmCurso(cursoId, "Curso com valor zero", 0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Valor da matrícula deve ser maior que zero*");
    }

    [Fact]
    public void Deve_obter_matricula_por_curso_id()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId, "Curso de DDD", 1000);

        // Act
        var matricula = aluno.ObterMatriculaPorCursoId(cursoId);

        // Assert
        matricula.Should().NotBeNull();
        matricula.CursoId.Should().Be(cursoId);
    }

    [Fact]
    public void Nao_deve_retornar_matricula_inexistente()
    {
        // Arrange
        var aluno = CriarAlunoValido();

        // Act
        Action act = () => aluno.ObterMatriculaPorCursoId(Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Matrícula não foi localizada*");
    }

    [Fact]
    public void Deve_permitir_multiplas_matriculas()
    {
        // Arrange
        var aluno = CriarAlunoValido();
        var curso1 = Guid.NewGuid();
        var curso2 = Guid.NewGuid();

        // Act
        aluno.MatricularEmCurso(curso1, "Curso Introdução ao DDD", 500);
        aluno.MatricularEmCurso(curso2, "Curso Avançado de DDD", 800);

        // Assert
        aluno.MatriculasCursos.Should().HaveCount(2);
    }
    #endregion

    #region Overrides
    [Fact]
    public void ToString_deve_retornar_nome_e_email()
    {
        // Arrange
        var aluno = CriarAlunoValido();

        // Act
        var texto = aluno.ToString();

        // Assert
        texto.Should().Contain(_nomeValido);
        texto.Should().Contain(_emailValido);
    }
    #endregion
}
