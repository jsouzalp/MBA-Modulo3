using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.Commands.CadastrarAluno;
public class CadastrarAlunoCommand : CommandRaiz
{
    public Guid AlunoId { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateTime DataNascimento { get; private set; }

    public CadastrarAlunoCommand(Guid alunoId, string nome, string email, DateTime dataNascimento)
    {
        DefinirRaizAgregacao(alunoId);
        AlunoId = alunoId;
        Nome = nome;
        Email = email;
        DataNascimento = dataNascimento;
    }
}
