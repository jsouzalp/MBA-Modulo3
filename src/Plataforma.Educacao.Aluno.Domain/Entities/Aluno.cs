using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Core.Validations;

namespace Plataforma.Educacao.Aluno.Domain.Entities;
public class Aluno : Entidade, IRaizAgregacao
{
    #region Atributos
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateTime DataNascimento { get; private set; }

    private readonly List<MatriculaCurso> _matriculasCursos = [];
    public IReadOnlyCollection<MatriculaCurso> MatriculasCursos  => _matriculasCursos.AsReadOnly(); 
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

    #region Metodos do Dominio
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

    public void MatricularEmCurso(Guid cursoId, string nomeCurso, decimal valor)
    {
        if (_matriculasCursos.Any(m => m.CursoId == cursoId)) { throw new DomainException("Aluno já está matriculado neste curso"); }

        var novaMatricula = new MatriculaCurso(Id, cursoId, nomeCurso, valor);
        _matriculasCursos.Add(novaMatricula);
    }

    public MatriculaCurso ObterMatriculaPorCursoId(Guid cursoId)
    {
        var matriculaCurso = _matriculasCursos.FirstOrDefault(m => m.CursoId == cursoId);
        if (matriculaCurso == null) { throw new DomainException("Matrícula não foi localizada"); }

        return matriculaCurso;
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
    public override string ToString() => $"{Nome} (Email: {Email})";
    #endregion
}
