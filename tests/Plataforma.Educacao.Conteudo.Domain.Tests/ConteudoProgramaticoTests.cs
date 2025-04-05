using FluentAssertions;
using Plataforma.Educacao.Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Domain.Tests
{
    public class ConteudoProgramaticoTests
    {
        [Fact]
        public void Deve_criar_conteudo_programatico_valido()
        {
            // Arrange
            var finalidade = "Formar o aluno em conceitos de DDD";
            var ementa = "Conceitos básicos e avançados de Domain Driven Design";

            // Act
            var conteudo = new ConteudoProgramatico(finalidade, ementa);

            // Assert
            conteudo.Should().NotBeNull();
            conteudo.Finalidade.Should().Be(finalidade);
            conteudo.Ementa.Should().Be(ementa);
        }

        [Fact]
        public void Nao_deve_criar_conteudo_com_finalidade_vazia()
        {
            // Arrange
            var finalidade = "";
            var ementa = "Ementa válida";

            // Act
            Action act = () => new ConteudoProgramatico(finalidade, ementa);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*Finalidade não pode ser vazio ou nulo*");
        }

        [Fact]
        public void Nao_deve_criar_conteudo_com_ementa_curta()
        {
            // Arrange
            var finalidade = "Finalidade válida";
            var ementa = "abc";

            // Act
            Action act = () => new ConteudoProgramatico(finalidade, ementa);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*Finalidade do conteúdo programático deve ter entre 10 e 100 caracteres*");
        }

        [Fact]
        public void Deve_alterar_finalidade_valida()
        {
            var conteudo = new ConteudoProgramatico("Finalidade válida", "Ementa válida");

            conteudo.AlterarFinalidade("Nova finalidade válida");

            conteudo.Finalidade.Should().Be("Nova finalidade válida");
        }

        [Fact]
        public void Nao_deve_alterar_para_finalidade_invalida()
        {
            var conteudo = new ConteudoProgramatico("Finalidade válida", "Ementa válida");

            Action act = () => conteudo.AlterarFinalidade("");

            act.Should().Throw<DomainException>()
                .WithMessage("*Finalidade não pode ser vazio ou nulo*");
        }
    }
}