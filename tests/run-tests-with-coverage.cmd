@echo off
echo === Executando testes com cobertura... ===
rmdir /s /q coverage-report
mkdir coverage-report
dotnet test Plataforma.Educacao.Aluno.Domain.Tests --collect:"XPlat Code Coverage"
for /r %%i in (*coverage.cobertura.xml) do copy %%i coverage-report\Plataforma.Educacao.Aluno.Domain.Tests\coverage0.xml
dotnet test Plataforma.Educacao.Conteudo.Application.Tests --collect:"XPlat Code Coverage"
for /r %%i in (*coverage.cobertura.xml) do copy %%i coverage-report\Plataforma.Educacao.Conteudo.Application.Tests\coverage1.xml
dotnet test Plataforma.Educacao.Conteudo.Domain.Tests --collect:"XPlat Code Coverage"
for /r %%i in (*coverage.cobertura.xml) do copy %%i coverage-report\Plataforma.Educacao.Conteudo.Domain.Tests\coverage2.xml
dotnet test Plataforma.Educacao.Conteudo.Repository.Tests --collect:"XPlat Code Coverage"
for /r %%i in (*coverage.cobertura.xml) do copy %%i coverage-report\Plataforma.Educacao.Conteudo.Repository.Tests\coverage3.xml
reportgenerator -reports:coverage-report\**\*.xml -targetdir:coverage-report\html -reporttypes:Html

echo === Acesse o relatorio em coverage-report\html\index.html ===
cd .\coverage-report\html
start index.html