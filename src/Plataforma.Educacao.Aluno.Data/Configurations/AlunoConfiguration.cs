using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Core.Data.Constants;
using System.Diagnostics.CodeAnalysis;

namespace Plataforma.Educacao.Aluno.Data.Configurations;

[ExcludeFromCodeCoverage]
public class AlunoConfiguration : IEntityTypeConfiguration<Domain.Entities.Aluno>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Aluno> builder)
    {
        #region Mapping columns
        builder.ToTable("Alunos");

        builder.HasKey(x => x.Id)
               .HasName("AlunosPK");

        builder.Property(x => x.Id)
               .HasColumnName("AlunoId")
               .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
               .IsRequired();

        builder.Property(x => x.CodigoUsuarioAutenticacao)
           .HasColumnName("CodigoUsuarioAutenticacao")
           .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
           .IsRequired();

        builder.Property(x => x.Nome)
               .HasColumnName("Nome")
               .HasColumnType(DatabaseTypeConstant.Varchar)
               .HasMaxLength(50)
               .UseCollation(DatabaseTypeConstant.Collate)
               .IsRequired();

        builder.Property(x => x.Email)
               .HasColumnName("Email")
               .HasColumnType(DatabaseTypeConstant.Varchar)
               .HasMaxLength(100)
               .UseCollation(DatabaseTypeConstant.Collate)
               .IsRequired();

        builder.Property(x => x.DataNascimento)
               .HasColumnName("DataNascimento")
               .HasColumnType(DatabaseTypeConstant.SmallDateTime)
               .IsRequired();
        #endregion

        #region Indexes
        builder.HasIndex(x => x.Nome).HasDatabaseName("AlunosNomeIDX");

        builder.HasIndex(x => x.Email)
               .IsUnique()
               .HasDatabaseName("AlunosEmailUK");
        #endregion

        #region Relationships
        builder.HasMany(x => x.MatriculasCursos)
           .WithOne(x => x.Aluno) // já existe essa propriedade
           .HasForeignKey(x => x.AlunoId)
           .HasConstraintName("AlunosMatriculaCursoFK")
           .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
