using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Conteudo.Application.Services;
using Plataforma.Educacao.Conteudo.Data.Contexts;
using Plataforma.Educacao.Conteudo.Data.Repositories;
using Plataforma.Educacao.Conteudo.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Plataforma.Educacao.Conteudo.Application.Configurations;

[ExcludeFromCodeCoverage]
public static class ConteudoProgramaticoConfiguration
{
    public static IServiceCollection ConfigurarConteudoApplication(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        return services
            .ConfigurarInjecoesDependenciasRepository()
            .ConfigurarInjecoesDependenciasApplication()
            .ConfigurarRepositorios(stringConexao, ehProducao);
    }

    private static IServiceCollection ConfigurarInjecoesDependenciasRepository(this IServiceCollection services)
    {
        services.AddScoped<ICursoRepository, CursoRepository>();
        return services;
    }

    private static IServiceCollection ConfigurarInjecoesDependenciasApplication(this IServiceCollection services)
    {
        services.AddScoped<ICursoAppService, CursoAppService>();
        services.AddScoped<IAulaAppService, AulaAppService>();
        return services;
    }

    private static IServiceCollection ConfigurarRepositorios(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        services.AddDbContext<ConteudoDbContext>(o =>
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
