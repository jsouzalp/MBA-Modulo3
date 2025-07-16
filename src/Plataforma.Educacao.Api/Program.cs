using FluentValidation;
using Plataforma.Educacao.Api.Configurations;
using Plataforma.Educacao.Api.Settings;
using Plataforma.Educacao.Conteudo.Application.Configurations;
using Plataforma.Educacao.Aluno.Application.Configurations;
using Plataforma.Educacao.Faturamento.Application.Configurations;
using System.Reflection;

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
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .ConfigurarJwt(appSettings.JwtSettings)
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .ConfigurarAutenticacao(appSettings.DatabaseSettings, builder.Environment.IsProduction())
            .ConfigurarConteudoApplication(appSettings.DatabaseSettings.ConnectionStringConteudoProgramatico, builder.Environment.IsProduction())
            .ConfigurarAlunoApplication(appSettings.DatabaseSettings.ConnectionStringAluno, builder.Environment.IsProduction())
            .ConfigurarFaturamentoApplication(appSettings.DatabaseSettings.ConnectionStringFaturamento, builder.Environment.IsProduction())
            .ConfigurarApi()
            .ConfigurarCors()
            .ConfigurarSwagger()
            .ConfigurarRegrasEspeciaisEndpoint();
        #endregion Extended Services configuration

        var app = builder.Build();
        app.ExecutarConfiguracaoAmbiente();
        app.Run();
    }
}