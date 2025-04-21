using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.DTO;
public class CertificadoDto
{
    public Guid Id { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public string PathCertificado { get; set; }
}