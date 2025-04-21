using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Plataforma.Educacao.Aluno.Application.Commands.CadastrarAluno;

public class CadastrarAlunoCommandValidator : AbstractValidator<CadastrarAlunoCommand>
{
    public CadastrarAlunoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(c => c.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email é obrigatório.").EmailAddress().WithMessage("Email inválido.");
        RuleFor(c => c.DataNascimento).LessThan(DateTime.Today).WithMessage("Data de nascimento deve ser no passado.");
    }
}
