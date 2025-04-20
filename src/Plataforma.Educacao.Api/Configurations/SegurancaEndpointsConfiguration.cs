using Microsoft.Extensions.Options;

namespace Plataforma.Educacao.Api.Configurations;

public static class SegurancaEndpointsConfiguration
{
    public static IServiceCollection ConfigurarRegrasEspeciaisEndpoint(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("ApenasAluno", policy => policy.RequireRole("Usuario"));

        services.AddAuthorizationBuilder()
            .AddPolicy("ApenasAdministrador", policy => policy.RequireRole("Administrador"));

        services.AddAuthorizationBuilder()
            .AddPolicy("AlunoOuAdministrador", policy => policy.RequireRole("Usuario", "Administrador"));

        return services;
    }
}
