@echo off
setlocal

echo === Limpando relatórios antigos ===

if exist tests\coverage-report rmdir /s /q tests\coverage-report

echo === Executando testes Plataforma.Educacao.Conteudo.Tests ===
dotnet test tests\Plataforma.Educacao.Conteudo.Tests --collect:"XPlat Code Coverage"

echo === Executando testes Plataforma.Educacao.Aluno.Tests ===
dotnet test tests\Plataforma.Educacao.Aluno.Tests --collect:"XPlat Code Coverage"

echo === Executando testes Plataforma.Educacao.Faturamento.Tests ===
dotnet test tests\Plataforma.Educacao.Faturamento.Tests --collect:"XPlat Code Coverage"

echo === Gerando Relatório de Cobertura ===
reportgenerator -reports:tests\**\coverage.cobertura.xml -targetdir:tests\coverage-report -reporttypes:Html

echo === Abrindo o Relatório ===
if exist tests\coverage-report\index.html (
    start tests\coverage-report\index.html
) else (
    echo !!! ERRO: Relatório HTML não foi gerado !!!
)

endlocal

