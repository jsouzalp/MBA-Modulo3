using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Core.Messages.Integration;
public class PagamentoConfirmadoEvent : EventoRaiz
{
    public Guid MatriculaId { get; init; }
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }

    public PagamentoConfirmadoEvent(Guid matriculaId, Guid alunoId, Guid cursoId)
    {
        DefinirRaizAgregacao(matriculaId);

        MatriculaId = matriculaId;
        AlunoId = alunoId;
        CursoId = cursoId;
    }
}