using FluentValidation;
using Plataforma.Educacao.Core.Messages.Integration;

namespace Plataforma.Educacao.Aluno.Application.Events.AtualizarPagamento
{
    public class PagamentoConfirmadoEventValidator : AbstractValidator<PagamentoConfirmadoEvent>
    {
        public PagamentoConfirmadoEventValidator()
        {
            RuleFor(c => c.MatriculaId).NotEqual(Guid.Empty).WithMessage("Matrícula do aluno é inválida");
            RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno é inválido");
            RuleFor(c => c.CursoId).NotEqual(Guid.Empty).WithMessage("Id do curso é inválido");
        }
    }
}
