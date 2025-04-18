using Plataforma.Educacao.Api.Configurations;
using Plataforma.Educacao.Api.Configurations.BCs;
using Plataforma.Educacao.Api.Settings;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Settings configuration

        var configuration = builder.Configuration;
        builder.Services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        #endregion Settings configuration

        #region Extended Services configuration

        builder.Services.AddHttpContextAccessor()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .ConfigurarAutenticacao(appSettings.DatabaseSettings, builder.Environment.IsProduction())
            .ConfigurarConteudoProgramatico(appSettings.DatabaseSettings, builder.Environment.IsProduction())
            .ConfigurarAluno(appSettings.DatabaseSettings, builder.Environment.IsProduction())
            .ConfigurarApi()
            .ConfigurarJwt(appSettings.JwtSettings)
            .ConfigurarCors()
            .ConfigurarSwagger();

        #endregion Extended Services configuration

        var app = builder.Build();
        app.ExecutarConfiguracaoAmbiente();
        app.Run();
    }
}