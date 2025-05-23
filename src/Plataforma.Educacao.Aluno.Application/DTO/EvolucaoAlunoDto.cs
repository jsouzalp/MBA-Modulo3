﻿namespace Plataforma.Educacao.Aluno.Application.DTO;
public class EvolucaoAlunoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }

    public ICollection<EvolucaoMatriculaCursoDto> MatriculasCursos { get; set; }
}