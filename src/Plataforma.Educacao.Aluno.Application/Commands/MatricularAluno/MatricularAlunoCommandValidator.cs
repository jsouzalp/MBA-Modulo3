using FluentValidation;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;

namespace Plataforma.Educacao.Aluno.Application.Commands.MatricularAluno;
public class MatricularAlunoCommandValidator : AbstractValidator<MatricularAlunoCommand>
{
    public MatricularAlunoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.CursoId).NotEqual(Guid.Empty).WithMessage("Id do curso inválido.");
    }
}
