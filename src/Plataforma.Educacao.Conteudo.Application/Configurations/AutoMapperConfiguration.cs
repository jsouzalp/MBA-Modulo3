using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
