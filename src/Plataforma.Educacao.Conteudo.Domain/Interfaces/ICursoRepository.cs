using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Conteudo.Domain.Interfaces;
public interface ICursoRepository : IRepository<Curso>
{
    Task AdicionarAsync(Curso curso);
    Task AtualizarAsync(Curso curso);
    Task DesativarAsync(Curso curso);
    Task<Curso> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Curso>> ObterTodosAsync();
    Task<IEnumerable<Curso>> ObterAtivosAsync();
    Task<bool> ExisteCursoComMesmoNomeAsync(string nome);
}
