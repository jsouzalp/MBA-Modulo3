using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Plataforma.Educacao.Autenticacao.Data.Contexts;
public class AutenticacaoDbContext : IdentityDbContext
{
    public AutenticacaoDbContext(DbContextOptions<AutenticacaoDbContext> options)
        : base(options)
    {
    }
}