namespace Plataforma.Educacao.Conteudo.Application.DTO;
public class CadastroCursoDto
{
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public DateTime? ValidoAte { get; set; }

    public string Finalidade { get; set; }
    public string Ementa { get; set; }
}
