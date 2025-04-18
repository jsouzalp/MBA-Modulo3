using FluentValidation;

namespace Plataforma.Educacao.Aluno.Application.Commands.ConcluirCurso;
public class ConcluirCursoCommandValidator : AbstractValidator<ConcluirCursoCommand>
{
    public ConcluirCursoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id da matrícula inválido.");
    }
}
