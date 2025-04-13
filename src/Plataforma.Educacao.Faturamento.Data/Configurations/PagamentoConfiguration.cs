using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Core.Data.Constants;
using Plataforma.Educacao.Faturamento.Domain.Entities;

namespace Plataforma.Educacao.Faturamento.Data.Configurations
{
    public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            #region Mapping columns
            builder.ToTable("Pagamentos");

            builder.HasKey(x => x.Id)
                    .HasName("PagamentosPK");

            builder.Property(x => x.Id)
                .HasColumnName("PagamentoId")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.MatriculaId)
                .HasColumnName("MatriculaId")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.Valor)
                .HasColumnName("Valor")
                .HasColumnType(DatabaseTypeConstant.Money)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(x => x.DataVencimento)
                .HasColumnName("DataVencimento")
                .HasColumnType(DatabaseTypeConstant.SmallDateTime)
                .IsRequired();

            builder.Property(x => x.DataPagamento)
                .HasColumnName("DataPagamento")
                .HasColumnType(DatabaseTypeConstant.SmallDateTime);

            builder.OwnsOne(c => c.Cartao, cc =>
            {
                cc.Property(c => c.Numero)
                    .HasColumnName("NumeroCartao")
                    .HasColumnType(DatabaseTypeConstant.Varchar)
                    .HasMaxLength(16)
                    .UseCollation(DatabaseTypeConstant.Collate)
                    .IsRequired();

                cc.Property(c => c.NomeTitular)
                    .HasColumnName("NomeTitularCartao")
                    .HasColumnType(DatabaseTypeConstant.Varchar)
                    .HasMaxLength(50)
                    .UseCollation(DatabaseTypeConstant.Collate)
                    .IsRequired();

                cc.Property(c => c.Validade)
                    .HasColumnName("ValidadeCartao")
                    .HasColumnType(DatabaseTypeConstant.Varchar)
                    .HasMaxLength(5)
                    .UseCollation(DatabaseTypeConstant.Collate)
                    .IsRequired();

                cc.Property(c => c.CVV)
                    .HasColumnName("CVVCartao")
                    .HasColumnType(DatabaseTypeConstant.Varchar)
                    .HasMaxLength(3)
                    .UseCollation(DatabaseTypeConstant.Collate)
                    .IsRequired();
            });

            builder.OwnsOne(c => c.StatusPagamento, sp =>
            {
                sp.Property(c => c.Status)
                    .HasColumnName("Status")
                    .HasColumnType(DatabaseTypeConstant.Byte)
                    .IsRequired();
            });
            #endregion Mapping columns

            #region Indexes
            builder.HasIndex(x => x.DataVencimento).HasDatabaseName("PagamentoDataVencimentoIDX");
            #endregion Indexes

            #region Relationships
            #endregion Relationships
        }
    }
}