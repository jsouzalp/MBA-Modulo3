namespace Plataforma.Educacao.Api.ViewModels.Aluno.Commands;
public class SolicitarCertificadoViewModel
{
    public Guid AlunoId { get; set; }
    public Guid MatriculaCursoId { get; set; }
    public string PathCertificado { get; set; }
}

