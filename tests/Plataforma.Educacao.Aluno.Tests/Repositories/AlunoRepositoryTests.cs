using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Data.Contexts;
using Plataforma.Educacao.Aluno.Data.Repositories;

namespace Plataforma.Educacao.Aluno.Tests.Repositories;
public class AlunoRepositoryTests
{
    #region Helpers
    private Domain.Entities.Aluno CriarAlunoValido()
    {
        return new Domain.Entities.Aluno("João da Silva", "joao@email.com", new DateTime(1990, 5, 12));
    }

    private AlunoRepository CriarRepository(out AlunoDbContext context)
    {
        var options = new DbContextOptionsBuilder<AlunoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new AlunoDbContext(options);
        return new AlunoRepository(context);
    }
    #endregion

    #region Ações na tabela - Aluno
    [Fact]
    public async Task Deve_adicionar_e_buscar_aluno_por_id()
    {
        var aluno = CriarAlunoValido();
        var repository = CriarRepository(out var context);

        await repository.AdicionarAsync(aluno);
        await context.SaveChangesAsync();

        var alunoDb = await repository.ObterPorIdAsync(aluno.Id);

        alunoDb.Should().NotBeNull();
        alunoDb.Email.Should().Be(aluno.Email);
    }

    [Fact]
    public async Task Deve_retornar_aluno_por_email()
    {
        var aluno = CriarAlunoValido();
        var repository = CriarRepository(out var context);

        await repository.AdicionarAsync(aluno);
        await context.SaveChangesAsync();

        var alunoDb = await repository.ObterPorEmailAsync(aluno.Email);

        alunoDb.Should().NotBeNull();
        alunoDb.Nome.Should().Be(aluno.Nome);
    }

    [Fact]
    public async Task Deve_verificar_existencia_de_email()
    {
        var aluno = CriarAlunoValido();
        var repository = CriarRepository(out var context);

        await repository.AdicionarAsync(aluno);
        await context.SaveChangesAsync();

        var existe = await repository.ExisteEmailAsync(aluno.Email);
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task Nao_deve_encontrar_aluno_com_email_inexistente()
    {
        var repository = CriarRepository(out var _);

        var aluno = await repository.ObterPorEmailAsync("email@inexistente.com");
        aluno.Should().BeNull();
    }

    [Fact]
    public async Task Deve_atualizar_aluno()
    {
        var aluno = CriarAlunoValido();
        var repository = CriarRepository(out var context);

        await repository.AdicionarAsync(aluno);
        await context.SaveChangesAsync();

        aluno.AtualizarNome("João Atualizado");
        await repository.AtualizarAsync(aluno);
        await context.SaveChangesAsync();

        var alunoAtualizado = await repository.ObterPorIdAsync(aluno.Id);
        alunoAtualizado.Nome.Should().Be("João Atualizado");
    }

    #endregion

    #region Overrides
    [Fact]
    public void Dispose_deve_encerrar_contexto_sem_excecao()
    {
        var repository = CriarRepository(out var context);
        var act = () => repository.Dispose();

        act.Should().NotThrow();
    }
    #endregion
}
