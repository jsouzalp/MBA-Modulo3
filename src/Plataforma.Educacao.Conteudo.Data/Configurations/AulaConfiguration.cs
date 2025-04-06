using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Data.Constants;

namespace Plataforma.Educacao.Conteudo.Data.Configurations
{
    public class AulaConfiguration : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            var nomeTabela = "Aulas";

            #region Mapping columns
            builder.ToTable(nomeTabela);

            builder.HasKey(x => x.Id)
                    .HasName("AulasPK");

            builder.Property(x => x.Id)
                .HasColumnName("AulaId")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.CursoId)
                .HasColumnName("CursoId")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasMaxLength(100)
                .UseCollation(DatabaseTypeConstant.Collate)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType(DatabaseTypeConstant.Boolean)
                //.HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CargaHoraria)
                .HasColumnName("CargaHoraria")
                .HasColumnType(DatabaseTypeConstant.Int16)
                .IsRequired();

            builder.Property(x => x.OrdemAula)
                .HasColumnName("OrdemAula")
                .HasColumnType(DatabaseTypeConstant.Byte)
                .IsRequired();
            #endregion Mapping columns

            #region Indexes
            builder.HasIndex(x => x.Descricao).HasDatabaseName("AulasDescricaoIDX");
            #endregion Indexes

            #region Constraints
            builder.ToTable(nomeTabela, tb =>
            {
                tb.HasCheckConstraint("AulasOrdemAulaConstraint", "[OrdemAula] > 0");
            });

            builder.ToTable(nomeTabela, tb =>
            {
                tb.HasCheckConstraint("AulasCargaHorariaConstraint", "[CargaHoraria] Between 1 and 5");
            });
            #endregion

            #region Relationships

            builder.HasOne(x => x.Curso)
                .WithMany(x => x.Aulas)
                .HasForeignKey(x => x.CursoId)
                .HasConstraintName("AulaCursoFK")
                .OnDelete(DeleteBehavior.NoAction);

            #endregion Relationships

        }
    }
}