namespace Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
public class ConcluirCursoCommand : CommandRaiz
{
    public Guid AlunoId { get; private set; }
    public Guid MatriculaCursoId { get; private set; }

    public ConcluirCursoCommand(Guid alunoId, Guid matriculaCursoId)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
    }
}
