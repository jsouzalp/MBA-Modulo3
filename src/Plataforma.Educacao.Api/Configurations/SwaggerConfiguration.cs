using Microsoft.OpenApi.Models;

namespace Plataforma.Educacao.Api.Configurations;
public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigurarSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            //c.SwaggerDoc("v1", new OpenApiInfo
            //{
            //    Title = "Plataforma Educacao Api",
            //    Version = "v1"
            //});
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        });

        return services;
    }
}
