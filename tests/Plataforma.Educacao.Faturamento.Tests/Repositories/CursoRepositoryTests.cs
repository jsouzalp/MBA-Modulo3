﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Faturamento.Data.Contexts;
using Plataforma.Educacao.Faturamento.Data.Repositories;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using Plataforma.Educacao.Faturamento.Domain.ValueObjects;

namespace Plataforma.Educacao.Faturamento.Tests.Repositories;
public class FaturamentoRepositoryTests
{
    #region Helpers
    private Pagamento CriarPagamento() => new(Guid.NewGuid(), 200, DateTime.Now.AddDays(5));
    private DadosCartao CriarCartao() => new DadosCartao("4111111111111111", "Jairo A Souza", "12/26", "123");

    private static FaturamentoRepository CriarRepository(out FaturamentoDbContext context)
    {
        var options = new DbContextOptionsBuilder<FaturamentoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new FaturamentoDbContext(options);
        return new FaturamentoRepository(context);
    }
    #endregion

    #region Métodos de Pagamento
    [Fact]
    public async Task Deve_adicionar_pagamento()
    {
        var pagamento = CriarPagamento();
        var repo = CriarRepository(out var context);

        await repo.AdicionarAsync(pagamento);
        await repo.UnitOfWork.Commit();

        context.Pagamentos.Should().ContainSingle(p => p.Id == pagamento.Id);
    }

    [Fact]
    public async Task Deve_atualizar_pagamento()
    {
        var pagamento = CriarPagamento();
        var repo = CriarRepository(out var context);

        await repo.AdicionarAsync(pagamento);
        await repo.UnitOfWork.Commit();

        pagamento.ConfirmarPagamento(null, "cod123", CriarCartao());
        await repo.AtualizarAsync(pagamento);
        await repo.UnitOfWork.Commit();

        var atualizado = await context.Pagamentos.FirstOrDefaultAsync(p => p.Id == pagamento.Id);
        atualizado.Cartao.Numero.Should().Be("4111111111111111");
        atualizado.Cartao.NomeTitular.Should().Be("Jairo A Souza");
        atualizado.Cartao.Validade.Should().Be("12/26");
        atualizado.Cartao.CVV.Should().Be("123");
    }

    [Fact]
    public async Task Deve_obter_pagamento_por_matricula()
    {
        var pagamento = CriarPagamento();
        var repo = CriarRepository(out var context);

        await repo.AdicionarAsync(pagamento);
        await repo.UnitOfWork.Commit();

        var encontrado = await repo.ObterPorMatriculaIdAsync(pagamento.MatriculaId);
        encontrado.Should().NotBeNull();
        encontrado.Id.Should().Be(pagamento.Id);
    }

    [Fact]
    public async Task Deve_retornar_null_quando_pagamento_nao_existir()
    {
        var repo = CriarRepository(out _);
        var idInexistente = Guid.NewGuid();

        var resultado = await repo.ObterPorMatriculaIdAsync(idInexistente);

        resultado.Should().BeNull();
    }
    #endregion
}
