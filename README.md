# 🎓 Plataforma de Educação - Sistema de Ensino Modular com DDD, TDD, CQRS e SOLID

## **1. Apresentação**
Bem-vindo ao repositório do projeto **Plataforma.Educacao**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **MÓDULO 3 - Arquitetura, Modelagem e Qualidade de Software**.
A aplicação foi construída utilizando conceitos modernos de arquitetura, como:
- **DDD - Domain Driven Design**
- **TDD - Test Driven Development**
- **CQRS - Command Query Responsibility Segregation**
- **SOLID / KISS / YAGNI**

### **Autor**
- **Jairo Azevedo de Souza**

## **2. Proposta do Projeto**

Criar uma plataforma robusta de ensino composta por três Bounded Contexts principais:

- **Gestão de Conteúdo Programático**
- **Gestão de Alunos e Matrículas**
- **Faturamento e Processamento de Pagamentos**

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação C# .NET 8**
- **Frameworks:**
  - ASP.NET Web API
  - Entity Framework Core
- **MediatR** 
- **Banco de Dados:** 
  - SQLite
  - Sqlserver
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Documentação da API:** 
  - Swagger
- **Coverlet + ReportGenerator (cobertura de testes)**

## **4. Arquitetura e Estrutura**

```plaintext
├── src
│   ├── Plataforma.Educacao.Aluno.Application        # AppServices do BC Aluno
│   ├── Plataforma.Educacao.Aluno.Data               # Repositório do BC Aluno
│   ├── Plataforma.Educacao.Aluno.Domain             # Domínio do BC Aluno
│   ├── Plataforma.Educacao.Api                      # API principal
│   ├── Plataforma.Educacao.Autenticacao.Data        # Repositório de autenticação
│   ├── Plataforma.Educacao.Conteudo.Application     # AppServices do BC Conteúdo Programático
│   ├── Plataforma.Educacao.Conteudo.Data            # Repositório do BC Conteúdo Programático
│   ├── Plataforma.Educacao.Conteudo.Domain          # Domínio do BC Conteúdo Programático
│   ├── Plataforma.Educacao.Core                     # Camada CORE da solução
│   ├── Plataforma.Educacao.Faturamento.Application  # AppServices do BC Faturamento
│   ├── Plataforma.Educacao.Faturamento.Data         # Repositório do BC Faturamento
│   └── Plataforma.Educacao.Faturamento.Domain       # Domínio do BC Faturamento
├── tests
│   ├── Plataforma.Educacao.Aluno.Tests              # Projeto de testes das camadas de Domínio + Repositório + AppServices do BC Aluno
│   ├── Plataforma.Educacao.Conteudo.Tests           # Projeto de testes das camadas de Domínio + Repositório + AppServices do BC Conteúdo Programático
│   └── Plataforma.Educacao.Faturamento.Tests        # Projeto de testes das camadas de Domínio + Repositório + AppServices do BC Faturamento
```

## **5. Funcionalidades**

- **BC Alunos**: Cadastrar, matricular e controlar histórico escolar do aluno
- **BC ConteúdoProgramático**: Cadastrar e controlar cursos e aulas
- **BC Pagamentos**: Realizar o controle de pagamento da matrícula do aluno
- **Autenticação e Autorização:** Diferenciação entre usuários comuns e administradores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- Angular (instalar o Node.js e o Angular CLI)
- SQLite
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   
   ```bash
   git clone -b https://github.com/jsouzalp/MBA-Modulo3.git
   ```
   
2. **Configuração do Banco de Dados:**
   
   - No arquivo `appsettings.json`, configure a string de conexão do SQLite.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a API:**
   
   ```bash
   cd .\src\FinPlanner360.Api
   dotnet run
   ```
   
   - Acesse a documentação da API em: http://localhost:5001/swagger
   
4. **Migrations (dotnet tools):**
   
   Autenticação
   ```bash
   dotnet ef migrations add InitialMigration --project .\src\Plataforma.Educacao.Autenticacao.Data --startup-project .\src\Plataforma.Educacao.Api --context AutenticacaoDbContext --output-dir Migrations
   ```
   
   BC Conteúdo Programático
   ```bash
   dotnet ef migrations add InitialMigration --project .\src\Plataforma.Educacao.Conteudo.Data --startup-project .\src\Plataforma.Educacao.Api --context ConteudoDbContext --output-dir Migrations
   ```
   
   BC Aluno
   ```bash
   dotnet ef migrations add InitialMigration --project .\src\Plataforma.Educacao.Aluno.Data --startup-project .\src\Plataforma.Educacao.Api --context AlunoDbContext --output-dir Migrations
   ```
   
   BC Faturamento
   ```bash
   dotnet ef migrations add InitialMigration --project .\src\Plataforma.Educacao.Faturamento.Data --startup-project .\src\Plataforma.Educacao.Api --context FaturamentoDbContext --output-dir Migrations
   ```
   
## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em: http://localhost:5001/swagger

## **9. Testes Unitários**

- 100% dos comandos, handlers, entidades e VOs possuem testes unitários
- Events handlers também testados com simulação de integração
- Consultas implementadas com mocks e validações de retorno
- Relatório gerado com `coverlet.collector` + `reportgenerator`

```cmd
.\tests\TestsCoverage.cmd
```
O comando gerará a pasta `.\tests\coverage-report\index.html"` com o resultado completo da cobertura.

## **10. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
