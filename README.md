# jsouzalp-MBA-Modulo3
MBA DevXpert [desenvolvedor.io]: Modulo 3 - Plataforma de Educação

# 🎓 Plataforma de Educação - Sistema de Ensino Modular com DDD, TDD, CQRS e SOLID

## **1. Apresentação**

Este repositório apresenta a implementação da **Plataforma de Educação Modular**, parte integrante do curso de **MBA DevXpert - Módulo 3**. A aplicação foi construída utilizando conceitos modernos de arquitetura, como:

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

- **.NET 8 / C#**
- **Entity Framework Core 8**
- **MediatR** (Mediador de Domínio)
- **FluentValidation**
- **xUnit / Moq / FluentAssertions**
- **Coverlet + ReportGenerator (cobertura de testes)**

## **4. Arquitetura e Estrutura**

```plaintext
├── src/
│   ├── Plataforma.Educacao.Api/               # API principal
│   ├── Plataforma.Educacao.Aluno/             # Domínio de alunos
│   ├── Plataforma.Educacao.Conteudo/          # Domínio de conteúdo programático
│   ├── Plataforma.Educacao.Faturamento/       # Domínio de faturamento
│   └── Plataforma.Educacao.Core/              # Cross-cutting concerns (Base classes, eventos, notificacoes)
│
├── tests/
│   ├── Plataforma.Educacao.Aluno.Tests/
│   ├── Plataforma.Educacao.Conteudo.Tests/
│   ├── Plataforma.Educacao.Faturamento.Tests/
│   └── cobertura/                             # Relatórios de cobertura de testes
```

## **5. Funcionalidades**

- 📘 **Alunos**: Cadastro, atualização, solicitação de certificado
- 📚 **Conteúdo**: Curso, aulas, histórico de aprendizado
- 💳 **Pagamentos**: Geração de link, confirmação e recusa
- ✅ **Validações de domínio fortes**, com testes cobrindo todas as entidades

## **6. Como Executar**

### **Requisitos:**
- .NET 8 SDK instalado
- SQLite (opcional - para banco local)

### **Executar testes com cobertura**
```cmd
./tests/TestsCoverage.cmd
```

O comando gerará a pasta `/tests/coverage-report/html/index.html` com o resultado completo da cobertura.

### **Executar API**
```cmd
cd src/Plataforma.Educacao.Api

dotnet run
```

Acesse via `https://localhost:5001/swagger`

## **7. Testes Automatizados**

- 100% dos comandos, handlers, entidades e VOs possuem testes unitários
- Events handlers também testados com simulação de integração
- Consultas implementadas com mocks e validações de retorno
- Relatório gerado com `coverlet.collector` + `reportgenerator`

## **8. Avaliação Final**

Este repositório reflete a aplicação prática dos conceitos de arquitetura limpa, separação de responsabilidades e testes unitários de ponta a ponta.

Para mais informações ou sugestões, utilize a área de Issues.

---

> Projeto desenvolvido para fins educacionais e avaliação do MBA DevXpert (.NET)
