namespace Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
public class AtualizarPagamentoMatriculaCommand : CommandRaiz
{
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }

    public AtualizarPagamentoMatriculaCommand(Guid alunoId, Guid cursoId)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        CursoId = cursoId;
    }
}
