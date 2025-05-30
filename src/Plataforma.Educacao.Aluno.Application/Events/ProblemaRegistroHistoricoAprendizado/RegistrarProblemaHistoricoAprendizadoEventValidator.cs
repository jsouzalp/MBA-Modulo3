﻿using FluentValidation;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoEvents;

namespace Plataforma.Educacao.Aluno.Application.Events.ProblemaRegistroHistoricoAprendizado;
public class RegistrarProblemaHistoricoAprendizadoEventValidator : AbstractValidator<RegistrarProblemaHistoricoAprendizadoEvent>
{
    public RegistrarProblemaHistoricoAprendizadoEventValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno é inválido");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id de matrícula do curso do aluno é inválida");
        RuleFor(c => c.AulaId).NotEqual(Guid.Empty).WithMessage("Id da aula é inválida");
    }
}
