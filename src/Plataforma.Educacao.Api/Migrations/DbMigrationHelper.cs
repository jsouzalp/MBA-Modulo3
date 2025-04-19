using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Data.Contexts;
using Plataforma.Educacao.Autenticacao.Data.Contexts;
using Plataforma.Educacao.Conteudo.Data.Contexts;
using Plataforma.Educacao.Faturamento.Data.Contexts;

namespace Plataforma.Educacao.Api.Migrations;

public static class DbMigrationHelper
{
    public static async Task AutocarregamentoDadosAsync(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await CarregamentoDadosAsync(services);
    }

    public static async Task CarregamentoDadosAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var identityContext = scope.ServiceProvider.GetRequiredService<AutenticacaoDbContext>();
        var conteudoContext = scope.ServiceProvider.GetRequiredService<ConteudoDbContext>();
        var alunoContext = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
        //var faturamentoContext =  scope.ServiceProvider.GetRequiredService<FaturamentoDbContext>();
        
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (env.IsDevelopment())
        {
            await identityContext.Database.MigrateAsync();
            await conteudoContext.Database.MigrateAsync();
            await alunoContext.Database.MigrateAsync();
            //await faturamentoContext.Database.MigrateAsync();
            await PopularDatabaseAsync(identityContext, conteudoContext, alunoContext, null, userManager);
        }
    }

    private static async Task PopularDatabaseAsync(AutenticacaoDbContext identityContext, ConteudoDbContext conteudoContext, AlunoDbContext aulaContext, FaturamentoDbContext faturamentoContext, UserManager<IdentityUser> userManager)
    {
        string roleAdmin = await CallIdentityRolesAsync(identityContext, "Administrador");
        string roleUsuario = await CallIdentityRolesAsync(identityContext, "Usuario");
        // throw new NotImplementedException();
    }

    private static async Task<string> CallIdentityRolesAsync(AutenticacaoDbContext identityContext, string role)
    {
        string roleId = Guid.NewGuid().ToString();
        identityContext.Roles.Add(new IdentityRole
        {
            Id = roleId,
            Name = role,
            NormalizedName = role,
            ConcurrencyStamp = DateTime.Now.ToString()
        });

        await identityContext.SaveChangesAsync();

        return roleId;
    }

}
