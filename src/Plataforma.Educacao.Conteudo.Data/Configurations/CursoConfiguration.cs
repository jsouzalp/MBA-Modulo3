using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Data.Constants;

namespace Plataforma.Educacao.Conteudo.Data.Configurations
{
    public class CursoConfiguration : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            #region Mapping columns
            builder.ToTable("Cursos");

            builder.HasKey(x => x.Id)
                    .HasName("CursosPK");

            builder.Property(x => x.Id)
                .HasColumnName("CursoId")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnName("Nome")
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasMaxLength(100)
                .UseCollation(DatabaseTypeConstant.Collate)
                .IsRequired();

            builder.Property(x => x.Valor)
                .HasColumnName("Valor")
                .HasColumnType(DatabaseTypeConstant.Money)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType(DatabaseTypeConstant.Boolean)
                //.HasDefaultValue(true)
                .IsRequired();

            //builder.Property(x => x.DataCriacao)
            //    .HasColumnName("DataCriacao")
            //    .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            //    .IsRequired();

            builder.Property(x => x.ValidoAte)
                .HasColumnName("ValidoAte")
                .HasColumnType(DatabaseTypeConstant.SmallDateTime);

            builder.OwnsOne(c => c.ConteudoProgramatico, cp =>
            {
                cp.Property(c => c.Finalidade)
                    .HasColumnName("Finalidade")
                    .HasColumnType(DatabaseTypeConstant.Varchar)
                    .HasMaxLength(100)
                    .UseCollation(DatabaseTypeConstant.Collate);
                    //.IsRequired();

                cp.Property(c => c.Ementa)
                    .HasColumnName("Ementa")
                    .HasColumnType(DatabaseTypeConstant.Varchar)
                    .HasMaxLength(4000)
                    .UseCollation(DatabaseTypeConstant.Collate);
                    //.IsRequired();
            });

            #endregion Mapping columns

            #region Indexes

            builder.HasIndex(x => x.Nome).HasDatabaseName("CursosNomeIDX");
            
            #endregion Indexes

            #region Relationships

            builder.HasMany(x => x.Aulas)
                .WithOne(x => x.Curso)
                .HasForeignKey(x => x.CursoId)
                .HasConstraintName("CursoAulaFK")
                .OnDelete(DeleteBehavior.NoAction);

            #endregion Relationships

        }
    }
}
