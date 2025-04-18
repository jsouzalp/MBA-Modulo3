using Microsoft.Extensions.DependencyInjection;
using Plataforma.Educacao.Aluno.Data.Repositories;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications;

namespace Plataforma.Educacao.Aluno.Application.Configurations;
public static class AlunoConfiguration
{
    public static IServiceCollection ConfigurarAlunoApplication(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        //services.AddScoped<IAlunoService, AlunoService>();
        //services.AddScoped<IMediatorHandler, MediatorHandler>();
        //services.AddScoped<IRequestHandler<MatricularAlunoCommand, bool>, MatricularAlunoCommandHandler>();
        //services.AddScoped<INotificationHandler<AlunoMatriculadoEvent>, AlunoMatriculadoEventHandler>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        return services;
    }

    //private static IServiceCollection ConfigurarInjecoesDependenciasRepository(this IServiceCollection services)
    //{
    //    services.AddScoped<ICursoRepository, CursoRepository>();
    //    return services;
    //}

    //private static IServiceCollection ConfigurarInjecoesDependenciasApplication(this IServiceCollection services)
    //{
    //    services.AddScoped<ICursoAppService, CursoAppService>();
    //    services.AddScoped<IAulaAppService, AulaAppService>();
    //    return services;
    //}

    //private static IServiceCollection ConfigurarRepositorios(this IServiceCollection services, string stringConexao, bool ehProducao)
    //{
    //    services.AddDbContext<ConteudoDbContext>(o =>
    //    {
    //        if (ehProducao)
    //        {
    //            o.UseSqlServer(stringConexao);
    //        }
    //        else
    //        {
    //            var connection = new SqliteConnection(stringConexao);
    //            connection.CreateCollation("LATIN1_GENERAL_CI_AI", (x, y) =>
    //            {
    //                if (x == null && y == null) return 0;
    //                if (x == null) return -1;
    //                if (y == null) return 1;

    //                // Comparação ignorando maiúsculas/minúsculas e acentos
    //                return string.Compare(x, y, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
    //            });

    //            o.UseSqlite(connection);
    //        }
    //    });

    //    return services;
    //}
}
