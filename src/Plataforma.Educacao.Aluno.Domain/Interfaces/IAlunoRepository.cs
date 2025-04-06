using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Domain.Interfaces
{
    public interface IAlunoRepository : IRepository<Entities.Aluno>
    {
    }
}
