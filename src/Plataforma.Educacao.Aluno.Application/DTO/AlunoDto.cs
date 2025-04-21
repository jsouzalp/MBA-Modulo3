using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.DTO;
public class AlunoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }

    public ICollection<MatriculaCursoDto> MatriculasCursos { get; set; }
}