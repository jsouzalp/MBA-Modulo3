using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Plataforma.Educacao.Aluno.Data.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Alunos",
            columns: table => new
            {
                AlunoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                CodigoUsuarioAutenticacao = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                Nome = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false, collation: "Latin1_General_CI_AI"),
                Email = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false, collation: "Latin1_General_CI_AI"),
                DataNascimento = table.Column<DateTime>(type: "SmallDateTime", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("AlunosPK", x => x.AlunoId);
            });

        migrationBuilder.CreateTable(
            name: "MatriculasCursos",
            columns: table => new
            {
                MatriculaCursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                AlunoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                CursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                NomeCurso = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false, collation: "Latin1_General_CI_AI"),
                Valor = table.Column<decimal>(type: "Money", precision: 10, scale: 2, nullable: false),
                DataMatricula = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                DataConclusao = table.Column<DateTime>(type: "SmallDateTime", nullable: true),
                EstadoMatricula = table.Column<int>(type: "TinyInt", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("MatriculasCursosPK", x => x.MatriculaCursoId);
                table.ForeignKey(
                    name: "MatriculasCursosAlunosFK",
                    column: x => x.AlunoId,
                    principalTable: "Alunos",
                    principalColumn: "AlunoId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Certificados",
            columns: table => new
            {
                CertificadoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                MatriculaCursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                DataSolicitacao = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                PathCertificado = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: false, collation: "Latin1_General_CI_AI")
            },
            constraints: table =>
            {
                table.PrimaryKey("CertificadosPK", x => x.CertificadoId);
                table.ForeignKey(
                    name: "MatriculasCursosCertificadosFK",
                    column: x => x.MatriculaCursoId,
                    principalTable: "MatriculasCursos",
                    principalColumn: "MatriculaCursoId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HistoricosAprendizado",
            columns: table => new
            {
                HistoricoAprendizadoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                CursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                AulaId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                NomeAula = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false),
                DataInicio = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                DataTermino = table.Column<DateTime>(type: "SmallDateTime", nullable: true),
                MatriculaCursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("HistoricoAprendizadoPK", x => x.HistoricoAprendizadoId);
                table.ForeignKey(
                    name: "FK_HistoricosAprendizado_MatriculasCursos_MatriculaCursoId",
                    column: x => x.MatriculaCursoId,
                    principalTable: "MatriculasCursos",
                    principalColumn: "MatriculaCursoId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "AlunosEmailUK",
            table: "Alunos",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "AlunosNomeIDX",
            table: "Alunos",
            column: "Nome");

        migrationBuilder.CreateIndex(
            name: "CertificadosMatriculaCursoIdIDX",
            table: "Certificados",
            column: "MatriculaCursoId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "HistoricosAprendizadoAulaIdIDX",
            table: "HistoricosAprendizado",
            column: "AulaId");

        migrationBuilder.CreateIndex(
            name: "HistoricosAprendizadoCursoIdIDX",
            table: "HistoricosAprendizado",
            column: "CursoId");

        migrationBuilder.CreateIndex(
            name: "IX_HistoricosAprendizado_MatriculaCursoId",
            table: "HistoricosAprendizado",
            column: "MatriculaCursoId");

        migrationBuilder.CreateIndex(
            name: "MatriculasCursosAlunoIdIDX",
            table: "MatriculasCursos",
            column: "AlunoId");

        migrationBuilder.CreateIndex(
            name: "MatriculasCursosCursoIdIDX",
            table: "MatriculasCursos",
            column: "CursoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Certificados");

        migrationBuilder.DropTable(
            name: "HistoricosAprendizado");

        migrationBuilder.DropTable(
            name: "MatriculasCursos");

        migrationBuilder.DropTable(
            name: "Alunos");
    }
}
