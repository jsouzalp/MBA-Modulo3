using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Core.Data.Constants;

namespace Plataforma.Educacao.Aluno.Data.Configurations;
public class MatriculaCursoConfiguration : IEntityTypeConfiguration<MatriculaCurso>
{
    public void Configure(EntityTypeBuilder<MatriculaCurso> builder)
    {
        #region Mapping columns
        builder.ToTable("MatriculasCursos");

        builder.HasKey(x => x.Id)
                .HasName("MatriculasCursosPK");

        builder.Property(x => x.Id)
            .HasColumnName("MatriculaCursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.AlunoId)
            .HasColumnName("AlunoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.CursoId)
            .HasColumnName("CursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.NomeCurso)
            .HasColumnName("NomeCurso")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .UseCollation(DatabaseTypeConstant.Collate)
            .IsRequired();
        
        builder.Property(x => x.Valor)
            .HasColumnName("Valor")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.DataMatricula)
            .HasColumnName("DataMatricula")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.DataConclusao)
            .HasColumnName("DataConclusao")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime);

        builder.Property(x => x.EstadoMatricula)
            .HasColumnName("EstadoMatricula")
            .HasColumnType(DatabaseTypeConstant.Byte)
            .IsRequired();

        builder.OwnsMany(x => x.HistoricoAprendizado, ha =>
        {
            ha.ToTable("HistoricosAprendizado");

            ha.WithOwner().HasForeignKey("MatriculaCursoId");

            ha.HasKey(x => x.Id)
                .HasName("HistoricoAprendizadoPK");

            ha.Property(x => x.Id).HasColumnName("HistoricoAprendizadoId").HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.CursoId).HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.AulaId).HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.NomeAula).HasColumnType(DatabaseTypeConstant.Varchar).HasMaxLength(100).IsRequired();
            ha.Property(x => x.DataInicio).HasColumnType(DatabaseTypeConstant.SmallDateTime).IsRequired();
            ha.Property(x => x.DataTermino).HasColumnType(DatabaseTypeConstant.SmallDateTime);

            ha.HasIndex(x => x.CursoId).HasDatabaseName("HistoricosAprendizadoCursoIdIDX");
            ha.HasIndex(x => x.AulaId).HasDatabaseName("HistoricosAprendizadoAulaIdIDX");
        });
        #endregion Mapping columns

        #region Indexes

        builder.HasIndex(x => x.AlunoId).HasDatabaseName("MatriculasCursosAlunoIdIDX");
        builder.HasIndex(x => x.CursoId).HasDatabaseName("MatriculasCursosCursoIdIDX");

        #endregion Indexes

        #region Relationships

        builder.HasOne(x => x.Certificado)
           .WithOne(x => x.MatriculaCurso)
           .HasForeignKey<Certificado>(x => x.MatriculaCursoId)
           .HasConstraintName("MatriculasCursosCertificadosFK")
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Aluno)
           .WithMany(x => x.MatriculasCursos)
           .HasForeignKey(x => x.AlunoId)
           .HasConstraintName("MatriculasCursosAlunosFK")
           .OnDelete(DeleteBehavior.Cascade);
        #endregion Relationships
    }
}
