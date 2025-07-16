using Microsoft.AspNetCore.Mvc;
using Plataforma.Educacao.Api.Authentications;
using Plataforma.Educacao.Api.Filters;

namespace Plataforma.Educacao.Api.Configurations;
public static class ApiConfiguration
{
    public static IServiceCollection ConfigurarApi(this IServiceCollection services)
    {
        services.AddScoped<IAppIdentityUser, AppIdentityUser>();

        services.AddControllers(options =>
        {
            options.Filters.Add<DomainExceptionFilter>();
            options.Filters.Add<ExceptionFilter>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        //services.AddApiVersioning(options =>
        //{
        //    options.AssumeDefaultVersionWhenUnspecified = true;
        //    options.DefaultApiVersion = new ApiVersion(1, 0);
        //    options.ReportApiVersions = true;
        //});

        //services.AddVersionedApiExplorer(options =>
        //{
        //    options.GroupNameFormat = "'v'VVV";
        //    options.SubstituteApiVersionInUrl = true;
        //});

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
        });

        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromDays(365);
            options.IncludeSubDomains = true;
            options.Preload = true;
        });

        return services;
    }
}