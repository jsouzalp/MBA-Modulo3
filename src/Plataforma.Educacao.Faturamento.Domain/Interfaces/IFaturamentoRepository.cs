using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Faturamento.Domain.Entities;

namespace Plataforma.Educacao.Faturamento.Domain.Interfaces
{
    public interface IFaturamentoRepository : IRepository<Pagamento>
    {
        Task AdicionarAsync(Pagamento pagamento);
        Task AtualizarAsync(Pagamento pagamento);
        Task<Pagamento> ObterPorMatriculaIdAsync(Guid matriculaId);
    }
}
