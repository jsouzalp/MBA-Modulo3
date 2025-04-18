using Plataforma.Educacao.Core.Messages;

namespace Plataforma.Educacao.Aluno.Application.Commands.ConcluirCurso;
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
