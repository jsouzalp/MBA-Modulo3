using Microsoft.Extensions.DependencyInjection;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Conteudo.Application.Services;
using Plataforma.Educacao.Conteudo.Data.Repositories;
using Plataforma.Educacao.Conteudo.Domain.Interfaces;

namespace Plataforma.Educacao.Conteudo.Application.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ConfigurarInjecoesDependencias(this IServiceCollection services)
        {
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoAppService, CursoAppService>();
            services.AddScoped<IAulaAppService, AulaAppService>();

            return services;
        }
    }
}
