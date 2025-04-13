using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Api.Settings;
using Plataforma.Educacao.Autenticacao.Data.Contexts;

namespace Plataforma.Educacao.Api.Configurations;
public static class AutenticacaoConfiguration
{
    public static IServiceCollection ConfigurarAutenticacao(this IServiceCollection services, DatabaseSettings databaseSettings, bool ehProducao)
    {
        services.AddDbContext<AutenticacaoDbContext>(o =>
        {
            if (ehProducao)
            {
                o.UseSqlServer(databaseSettings.ConnectionStringIdentity);
            }
            else
            {
                o.UseSqlite(databaseSettings.ConnectionStringIdentity);
            }
        });

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 0;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<AutenticacaoDbContext>()
        .AddRoles<IdentityRole>()
        .AddDefaultTokenProviders();

        return services;
    }
}
