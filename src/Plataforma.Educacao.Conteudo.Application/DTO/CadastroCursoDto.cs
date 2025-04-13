using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Application.DTO;
public class CadastroCursoDto
{
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public DateTime? ValidoAte { get; set; }

    public string Finalidade { get; set; }
    public string Ementa { get; set; }

    //public List<AulaDto> Aulas { get; set; } = [];
}
