using Plataforma.Educacao.Conteudo.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Application.Interfaces;
public interface IAulaAppService
{
    Task<Guid> AdicionarAulaAsync(Guid cursoId, AulaDto dto);
    Task AtualizarAulaAsync(Guid cursoId, AulaDto dto);
    Task RemoverAulaAsync(Guid cursoId, Guid aulaId);
}
