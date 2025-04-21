using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Messages;

namespace Plataforma.Educacao.Aluno.Application.Commands.SolicitarCertificado;
public class SolicitarCertificadoCommand : CommandRaiz
{
    public Guid AlunoId { get; private set; }
    public Guid MatriculaCursoId { get; private set; }
    public string PathCertificado { get; private set; }

    public SolicitarCertificadoCommand(Guid alunoId, Guid matriculaCursoId, string pathCertificado)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
        PathCertificado = pathCertificado;
    }
}
