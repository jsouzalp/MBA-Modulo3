using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Api.ViewModels.Aluno.Queries;
public class AlunoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }

    public ICollection<MatriculaCursoViewModel> MatriculasCursos { get; set; }
}