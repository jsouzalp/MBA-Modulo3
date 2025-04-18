using FluentValidation;

namespace Plataforma.Educacao.Aluno.Application.Commands.MatricularAluno;
public class MatricularAlunoCommandValidator : AbstractValidator<MatricularAlunoCommand>
{
    public MatricularAlunoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty);
        RuleFor(c => c.CursoId).NotEqual(Guid.Empty);
    }
}
