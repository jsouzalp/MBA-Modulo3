using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Conteudo.Data.Contexts;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Conteudo.Domain.Interfaces;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Conteudo.Data.Repositories;
public class CursoRepository(ConteudoDbContext context) : ICursoRepository
{
    private readonly ConteudoDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    public async Task AdicionarAsync(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
    }

    public async Task AtualizarAsync(Curso curso)
    {
        _context.Cursos.Update(curso);
        await Task.CompletedTask;
    }

    public async Task DesativarAsync(Curso curso)
    {
        curso.DesativarCurso();
        _context.Cursos.Update(curso);
        await Task.CompletedTask;
    }

    public async Task<Curso> ObterPorIdAsync(Guid id)
    {
        return await _context.Cursos
            .Include(c => c.ConteudoProgramatico)
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Curso>> ObterTodosAsync()
    {
        return await _context.Cursos
            .AsNoTracking()
            .Include(c => c.ConteudoProgramatico)
            .Include(c => c.Aulas)
            .ToListAsync();
    }

    public async Task<IEnumerable<Curso>> ObterAtivosAsync()
    {
        return await _context.Cursos
            .AsNoTracking()
            .Where(c => c.Ativo)
            .Include(c => c.ConteudoProgramatico)
            .Include(c => c.Aulas)
            .ToListAsync();
    }

    public async Task<bool> ExisteCursoComMesmoNomeAsync(string nome)
    {
        return await _context.Cursos
            .AsNoTracking()
            .AnyAsync(c => c.Nome == nome);
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}
