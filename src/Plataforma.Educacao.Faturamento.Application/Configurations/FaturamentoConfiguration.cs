using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoCommands;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Core.Messages.DomainHandlers;
using Plataforma.Educacao.Faturamento.Application.Commands.RealizarPagamento;
using Plataforma.Educacao.Faturamento.Application.Events.GerarLinkPagamento;
using Plataforma.Educacao.Faturamento.Data.Contexts;
using Plataforma.Educacao.Faturamento.Data.Repositories;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Plataforma.Educacao.Faturamento.Application.Configurations;

[ExcludeFromCodeCoverage]
public static class FaturamentoConfiguration
{
    public static IServiceCollection ConfigurarFaturamentoApplication(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        return services
            .ConfigurarInjecoesDependenciasRepository()
            .ConfigurarInjecoesDependenciasApplication()
            .ConfigurarRepositorios(stringConexao, ehProducao);
    }

    private static IServiceCollection ConfigurarInjecoesDependenciasRepository(this IServiceCollection services)
    {
        services.AddScoped<IFaturamentoRepository, FaturamentoRepository>();
        return services;
    }

    private static IServiceCollection ConfigurarInjecoesDependenciasApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();
        services.AddScoped<INotificationHandler<GerarLinkPagamentoEvent>, GerarLinkPagamentoEventHandler>();

        services.AddScoped<IRequestHandler<RealizarPagamentoCommand, bool>, RealizarPagamentoCommandHandler>();

        return services;
    }

    private static IServiceCollection ConfigurarRepositorios(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        services.AddDbContext<FaturamentoDbContext>(o =>
        {
            if (ehProducao)
            {
                o.UseSqlServer(stringConexao);
            }
            else
            {
                var connection = new SqliteConnection(stringConexao);
                connection.CreateCollation("LATIN1_GENERAL_CI_AI", (x, y) =>
                {
                    if (x == null && y == null) return 0;
                    if (x == null) return -1;
                    if (y == null) return 1;

                    // Comparação ignorando maiúsculas/minúsculas e acentos
                    return string.Compare(x, y, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                });

                o.UseSqlite(connection);
            }
        });

        return services;
    }
}
