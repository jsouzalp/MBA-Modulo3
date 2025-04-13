using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Application.DTO;
public class AtualizacaoCursoDto
{
    public Guid CursoId { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public DateTime? ValidoAte { get; set; }
    public bool Ativo { get; set; }
    public string Finalidade { get; set; }
    public string Ementa { get; set; }
}
