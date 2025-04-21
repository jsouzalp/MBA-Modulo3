using Plataforma.Educacao.Api.Settings;
using Plataforma.Educacao.Aluno.Application.Configurations;

namespace Plataforma.Educacao.Api.Configurations.BCs
{
    public static class AlunoConfiguration
    {
        public static IServiceCollection ConfigurarAluno(this IServiceCollection services, DatabaseSettings databaseSettings, bool ehProducao)
        {
            services.ConfigurarAlunoApplication(databaseSettings.ConnectionStringAluno, ehProducao);
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
}
