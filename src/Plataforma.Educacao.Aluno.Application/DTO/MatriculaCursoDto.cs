using Plataforma.Educacao.Aluno.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.DTO;
public class MatriculaCursoDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public bool PagamentoPodeSerRealizado { get; set; }
    public string NomeCurso { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataMatricula { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string EstadoMatricula { get; set; }
    public CertificadoDto Certificado { get; set; }
}
