namespace Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
public class MatricularAlunoCommand : CommandRaiz
{
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }

    public MatricularAlunoCommand(Guid alunoId, Guid cursoId)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        CursoId = cursoId;
    }
}
