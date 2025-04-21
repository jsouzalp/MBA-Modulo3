using FluentValidation;

namespace Plataforma.Educacao.Aluno.Application.Commands.AtualizarPagamento
{
    public class AtualizarPagamentoMatriculaCommandValidator : AbstractValidator<AtualizarPagamentoMatriculaCommand>
    {
        public AtualizarPagamentoMatriculaCommandValidator()
        {
            RuleFor(c => c.AlunoId).NotEqual(Guid.Empty);
            RuleFor(c => c.CursoId).NotEqual(Guid.Empty);
        }
    }
}
