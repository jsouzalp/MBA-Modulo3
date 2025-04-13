using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Faturamento.Domain.Entities;

namespace Plataforma.Educacao.Faturamento.Domain.Interfaces
{
    public interface IPagamentoQueryRepository : IRepository<Pagamento>
    {
        //Task<PagamentoDetalhadoDto> ObterDetalhadoPorIdAsync(Guid pagamentoId);
        //Task<IReadOnlyCollection<PagamentoDetalhadoDto>> ObterPorMatriculaAsync(Guid matriculaId);
        //Task<IReadOnlyCollection<PagamentoDetalhadoDto>> ObterPendentesVencidosAsync(DateTime? ate = null);
    }
}
