namespace Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
public class PagamentoConfirmadoEvent : EventoRaiz
{
    public Guid MatriculaCursoId { get; init; }
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }

    public PagamentoConfirmadoEvent(Guid matriculaCursoId, Guid alunoId, Guid cursoId)
    {
        DefinirRaizAgregacao(matriculaCursoId);

        MatriculaCursoId = matriculaCursoId;
        AlunoId = alunoId;
        CursoId = cursoId;
    }
}