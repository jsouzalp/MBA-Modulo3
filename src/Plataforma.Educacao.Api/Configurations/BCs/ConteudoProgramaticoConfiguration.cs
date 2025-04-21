using Plataforma.Educacao.Api.Settings;
using Plataforma.Educacao.Conteudo.Application.Configurations;

namespace Plataforma.Educacao.Api.Configurations.BCs;

public static class ConteudoProgramaticoConfiguration
{
    public static IServiceCollection ConfigurarConteudoProgramatico(this IServiceCollection services, DatabaseSettings databaseSettings, bool ehProducao)
    {
        services.ConfigurarConteudoApplication(databaseSettings.ConnectionStringConteudoProgramatico, ehProducao);
        //services.AddIdentityRepositories(databaseSettings, isProduction);

        //#region Business injection

        //// Validations
        //services.AddValidatorsFromAssemblyContaining<UserValidation>();
        //services.AddScoped(typeof(IValidationFactory<>), typeof(ValidationFactory<>));

        //#endregion Business injection

        ////Swagger
        //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Swagger.ConfigureSwaggerOptions>();

        return services;
    }
}
