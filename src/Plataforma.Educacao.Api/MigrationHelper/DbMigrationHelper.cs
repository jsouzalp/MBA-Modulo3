using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Data.Contexts;
using Plataforma.Educacao.Autenticacao.Data.Contexts;
using Plataforma.Educacao.Conteudo.Data.Contexts;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Faturamento.Data.Contexts;

namespace Plataforma.Educacao.Api.Migrations;

public static class DbMigrationHelper
{
    private static AutenticacaoDbContext _identityContext = null;
    private static ConteudoDbContext _conteudoContext = null;
    private static AlunoDbContext _alunoContext = null;
    private static FaturamentoDbContext _faturamentoContext = null;
    private static UserManager<IdentityUser> _userManager = null;

    public static async Task AutocarregamentoDadosAsync(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await CarregamentoDadosAsync(services);
    }

    public static async Task CarregamentoDadosAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        _identityContext = scope.ServiceProvider.GetRequiredService<AutenticacaoDbContext>();
        _conteudoContext = scope.ServiceProvider.GetRequiredService<ConteudoDbContext>();
        _alunoContext = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
        _faturamentoContext =  scope.ServiceProvider.GetRequiredService<FaturamentoDbContext>();        
        _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (env.IsDevelopment())
        {
            await _identityContext.Database.MigrateAsync();
            await _conteudoContext.Database.MigrateAsync();
            await _alunoContext.Database.MigrateAsync();
            await _faturamentoContext.Database.MigrateAsync();
            await PopularDatabaseAsync();
        }
    }

    private static async Task PopularDatabaseAsync()
    {
        string roleAdminId = await CriarRegraAcessoAsync(_identityContext, "Administrador");
        string roleUsuarioId = await CriarRegraAcessoAsync(_identityContext, "Usuario");

        await CriarUsuarioAsync("jsouza.nz@gmail.com", "Password@2025", "Jairo Azevedo", new DateTime(1973, 12, 31), roleAdminId, true);
        await CriarUsuarioAsync("cath.nz@gmail.com", "Password@2025", "Cath Oliveira", new DateTime(2000, 12, 31), roleUsuarioId, false);
        await CriarUsuarioAsync("lari.nz@gmail.com", "Password@2025", "Larissa Souza", new DateTime(2000, 12, 31), roleUsuarioId, false);
    }

    private static async Task<string> CriarRegraAcessoAsync(AutenticacaoDbContext identityContext, string role)
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

    private static async Task CriarUsuarioAsync(string email, string senha, string nome, DateTime dataNascimento, string roleId, bool ehAdmin)
    {
        var identityUser = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(identityUser, senha);

        if (result.Succeeded)
        {
            #region Roles
            _identityContext.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = identityUser.Id.ToString()
            });

            await _identityContext.SaveChangesAsync();
            #endregion Roles

            #region Data
            Guid userId = Guid.Parse(identityUser.Id);
            if (ehAdmin)
            {
                await CriarCursoAsync();
            }
            else
            {
                await CriarAlunoAsync(Guid.Parse(identityUser.Id), nome, email, dataNascimento);
            }
            #endregion
        }
    }

    private static async Task CriarCursoAsync()
    {
        ConteudoProgramatico conteudoCurso1 = new ConteudoProgramatico("Capacitar os alunos para desenvolvimento de aplicações web modernas utilizando .NET",
            "Neste curso, o aluno irá aprender a desenvolver aplicações web full stack, desde o backend em .NET até o frontend com Angular.");
        Curso curso1 = new Curso("Curso de Desenvolvimento Full Stack com .NET e Angular", 3500m, DateTime.Today.AddYears(2), conteudoCurso1);
        curso1.AdicionarAula("1 - Fundamentos do .NET", 1, 1, "https://meucurso.com.mz/aula1");
        curso1.AdicionarAula("2 - Criando APIs REST com ASP.NET Core", 2, 2, "https://meucurso.com.mz/aula2");
        curso1.AdicionarAula("3 - Introdução ao Angular", 3, 3, "https://meucurso.com.mz/aula3");
        curso1.AdicionarAula("4 - Comunicação entre Angular e APIs", 2, 4, "https://meucurso.com.mz/aula4");
        curso1.AdicionarAula("5 - Deploy de aplicações Full Stack", 1, 5, "https://meucurso.com.mz/aula5");

        ConteudoProgramatico conteudoCurso2 = new ConteudoProgramatico("Ensinar práticas ágeis e gestão de projetos usando Scrum e Kanban",
            "Neste curso, o aluno irá entender como organizar e gerenciar times ágeis, entregando valor continuamente usando frameworks ágeis.");
        Curso curso2 = new Curso("Curso de Gestão Ágil de Projetos com Scrum e Kanban", 2800m, DateTime.Today.AddYears(2), conteudoCurso2);
        curso2.AdicionarAula("1 - Fundamentos do Manifesto Ágil", 1, 1, "https://meucurso.com.mz/aula1");
        curso2.AdicionarAula("2 - Estrutura do Scrum", 2, 2, "https://meucurso.com.mz/aula2");
        curso2.AdicionarAula("3 - Papéis no Scrum", 1, 3, "https://meucurso.com.mz/aula3");
        curso2.AdicionarAula("4 - Introdução ao Kanban", 1, 4, "https://meucurso.com.mz/aula4");
        curso2.AdicionarAula("5 - Como combinar Scrum e Kanban", 2, 5, "https://meucurso.com.mz/aula5");

        ConteudoProgramatico conteudoCurso3 = new ConteudoProgramatico("Formar profissionais para o mercado de análise de dados utilizando ferramentas modernas",
            "Neste curso, o aluno irá aprender desde a modelagem de dados até a construção de dashboards e análise de performance com Power BI.");
        Curso curso3 = new Curso("Curso de Análise de Dados com Power BI e SQL Server", 3200m, DateTime.Today.AddYears(2), conteudoCurso3);
        curso3.AdicionarAula("1 - Introdução à Análise de Dados", 1, 1, "https://meucurso.com.mz/aula1");
        curso3.AdicionarAula("2 - Fundamentos do SQL Server para análise", 3, 2, "https://meucurso.com.mz/aula2");
        curso3.AdicionarAula("3 - Modelagem de Dados no Power BI", 2, 3, "https://meucurso.com.mz/aula3");
        curso3.AdicionarAula("4 - Criação de Dashboards Interativos", 2, 4, "https://meucurso.com.mz/aula4");
        curso3.AdicionarAula("5 - Publicação e Compartilhamento de Relatórios", 2, 5, "https://meucurso.com.mz/aula5");

        await _conteudoContext.Cursos.AddAsync(curso1);
        await _conteudoContext.Cursos.AddAsync(curso2);
        await _conteudoContext.Cursos.AddAsync(curso3);
        await _conteudoContext.SaveChangesAsync();
    }

    private static async Task CriarAlunoAsync(Guid identityId, string nome, string email, DateTime dataNascimento)
    {
        var listaCursos = _conteudoContext.Cursos.ToList();
        var listaAulas = _conteudoContext.Aulas.ToList();

        Aluno.Domain.Entities.Aluno aluno = new Aluno.Domain.Entities.Aluno(nome, email, dataNascimento);
        aluno.IdentificarCodigoUsuarioNoSistema(identityId);

        var finalizarCurso = true;
        foreach (var curso in listaCursos)
        {
            aluno.MatricularEmCurso(curso.Id, curso.Nome, curso.Valor);
            var matriculaCurso = aluno.MatriculasCursos.Last();
            aluno.AtualizarPagamentoMatricula(matriculaCurso.Id);

            foreach (var aula in listaAulas.Where(x => x.CursoId == curso.Id).ToList())
            {
                aluno.RegistrarHistoricoAprendizado(matriculaCurso.Id, aula.Id, aula.Descricao, DateTime.Today);
            }

            if (finalizarCurso)
            {
                finalizarCurso = false;
                aluno.ConcluirCurso(matriculaCurso.Id);
                aluno.RequisitarCertificadoConclusao(matriculaCurso.Id, $"/var/tmp/alunos/{aluno.Id}/certificados/{curso.Id}.pdf");
            }
        }

        _alunoContext.Alunos.Add(aluno);
        await _alunoContext.SaveChangesAsync();
    }
}
