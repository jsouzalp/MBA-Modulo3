﻿using FluentValidation;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;

namespace Plataforma.Educacao.Aluno.Application.Commands.SolicitarCertificado;
public class SolicitarCertificadoCommandValidator : AbstractValidator<SolicitarCertificadoCommand>
{
    public SolicitarCertificadoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id da matrícula inválido.");
        RuleFor(c => c.PathCertificado).NotEmpty().WithMessage("O caminho do certificado é obrigatório.");
    }
}
