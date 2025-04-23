using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Plataforma.Educacao.Conteudo.Data.Migrations;
/// <inheritdoc />

[ExcludeFromCodeCoverage]
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Cursos",
            columns: table => new
            {
                CursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                Nome = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false, collation: "Latin1_General_CI_AI"),
                Valor = table.Column<decimal>(type: "Money", precision: 10, scale: 2, nullable: false),
                Ativo = table.Column<bool>(type: "Bit", nullable: false),
                ValidoAte = table.Column<DateTime>(type: "SmallDateTime", nullable: true),
                Finalidade = table.Column<string>(type: "Varchar", maxLength: 100, nullable: true, collation: "Latin1_General_CI_AI"),
                Ementa = table.Column<string>(type: "Varchar", maxLength: 4000, nullable: true, collation: "Latin1_General_CI_AI")
            },
            constraints: table =>
            {
                table.PrimaryKey("CursosPK", x => x.CursoId);
            });

        migrationBuilder.CreateTable(
            name: "Aulas",
            columns: table => new
            {
                AulaId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                CursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                Descricao = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false, collation: "Latin1_General_CI_AI"),
                Ativo = table.Column<bool>(type: "Bit", nullable: false),
                CargaHoraria = table.Column<short>(type: "SmallInt", nullable: false),
                OrdemAula = table.Column<byte>(type: "TinyInt", nullable: false),
                Url = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: false, collation: "Latin1_General_CI_AI")
            },
            constraints: table =>
            {
                table.PrimaryKey("AulasPK", x => x.AulaId);
                table.CheckConstraint("AulasCargaHorariaConstraint", "[CargaHoraria] Between 1 and 5");
                table.CheckConstraint("AulasOrdemAulaConstraint", "[OrdemAula] > 0");
                table.ForeignKey(
                    name: "CursoAulaFK",
                    column: x => x.CursoId,
                    principalTable: "Cursos",
                    principalColumn: "CursoId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "AulasDescricaoIDX",
            table: "Aulas",
            column: "Descricao");

        migrationBuilder.CreateIndex(
            name: "IX_Aulas_CursoId",
            table: "Aulas",
            column: "CursoId");

        migrationBuilder.CreateIndex(
            name: "CursosNomeIDX",
            table: "Cursos",
            column: "Nome");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Aulas");

        migrationBuilder.DropTable(
            name: "Cursos");
    }
}
