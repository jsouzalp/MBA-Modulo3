using Microsoft.Extensions.DependencyInjection;

namespace Plataforma.Educacao.Conteudo.Application.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection ConfigurarAutoMapper(this IServiceCollection service)
        {
            //service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return service;
        }
    }
}
