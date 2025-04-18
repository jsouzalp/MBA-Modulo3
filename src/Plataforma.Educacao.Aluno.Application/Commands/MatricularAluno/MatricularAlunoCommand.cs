using Plataforma.Educacao.Core.Messages;

namespace Plataforma.Educacao.Aluno.Application.Commands.MatricularAluno;
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
