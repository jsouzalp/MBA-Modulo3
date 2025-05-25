using Moq;
using FluentAssertions;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoCommands;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;
using Plataforma.Educacao.Faturamento.Domain.ValueObjects;
using Plataforma.Educacao.Aluno.Application.Interfaces;
using Plataforma.Educacao.Faturamento.Application.Commands.RealizarPagamento;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Core.SharedDto.Aluno;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Plataforma.Educacao.Faturamento.Tests.Applications.Commands;
public class RealizarPagamentoCommandHandlerTests
{
    private readonly Mock<IFaturamentoRepository> _faturamentoRepositoryMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly RealizarPagamentoCommandHandler _handler;

    public RealizarPagamentoCommandHandlerTests()
    {
        _faturamentoRepositoryMock = new Mock<IFaturamentoRepository>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _faturamentoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        _handler = new RealizarPagamentoCommandHandler(
            _faturamentoRepositoryMock.Object,
            _mediatorHandlerMock.Object
        );
    }

    private RealizarPagamentoCommand CriarComandoValido()
    {
        var matriculaId = Guid.NewGuid();
        decimal valor = 2500.00m;

        return new RealizarPagamentoCommand(matriculaId, 
            new MatriculaCursoDto
            {
                Id = matriculaId,
                AlunoId = Guid.NewGuid(),
                CursoId = Guid.NewGuid(),
                Valor = valor,
                PagamentoPodeSerRealizado = true
            }, 
            valor, 
            "5493813493498874", 
            "JAIRO A SOUZA", 
            "12/26", 
            "123");
    }

    private RealizarPagamentoCommand CriarComandoSemMatriculaCurso()
    {
        var matriculaId = Guid.NewGuid();
        decimal valor = 2500.00m;

        return new RealizarPagamentoCommand(matriculaId,
            null,
            valor,
            "5493813493498874",
            "JAIRO A SOUZA",
            "12/26",
            "123");
    }

    [Fact]
    public async Task Deve_retornar_false_quando_comando_invalido()
    {
        var comando = new RealizarPagamentoCommand(Guid.Empty, null, 0.00m, "", "", "", "");
        var resultado = await _handler.Handle(comando, CancellationToken.None);

        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_retornar_false_quando_matricula_nao_existir()
    {
        var comando = CriarComandoSemMatriculaCurso();

        _faturamentoRepositoryMock.Setup(r => r.ObterPorMatriculaIdAsync(comando.MatriculaCursoId))
            .ReturnsAsync((Pagamento?)null);

        //_alunoQueryServiceMock.Setup(q => q.ObterInformacaoMatriculaCursoAsync(comando.MatriculaCursoId))
        //    .ReturnsAsync((MatriculaCursoDto?)null);

        var resultado = await _handler.Handle(comando, CancellationToken.None);

        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_confirmar_pagamento_com_sucesso()
    {
        var comando = CriarComandoValido();

        _faturamentoRepositoryMock.Setup(r => r.ObterPorMatriculaIdAsync(comando.MatriculaCursoId))
            .ReturnsAsync((Pagamento?)null);

        //_alunoQueryServiceMock.Setup(q => q.ObterInformacaoMatriculaCursoAsync(comando.MatriculaCursoId))
        //    .ReturnsAsync(new MatriculaCursoDto
        //    {
        //        Id = comando.MatriculaCursoId,
        //        AlunoId = Guid.NewGuid(),
        //        CursoId = Guid.NewGuid(),
        //        Valor = comando.Valor,
        //        PagamentoPodeSerRealizado = true
        //    });

        var resultado = await _handler.Handle(comando, CancellationToken.None);

        resultado.Should().BeTrue();
        _faturamentoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Pagamento>()), Times.Once);
        _mediatorHandlerMock.Verify(m => m.PublicarEvento(It.IsAny<PagamentoConfirmadoEvent>()), Times.Once);
    }

    [Fact]
    public async Task Deve_retornar_false_quando_pagamento_ja_confirmado()
    {
        var comando = CriarComandoValido();
        var pagamento = new Pagamento(comando.MatriculaCursoId, comando.Valor, DateTime.Now);
        pagamento.ConfirmarPagamento(DateTime.Now, "ABCUIYKJHKJSAHDKAS", new DadosCartao(comando.NumeroCartao, comando.NomeTitularCartao, comando.ValidadeCartao, comando.CvvCartao));

        _faturamentoRepositoryMock.Setup(r => r.ObterPorMatriculaIdAsync(comando.MatriculaCursoId))
            .ReturnsAsync(pagamento);

        var resultado = await _handler.Handle(comando, CancellationToken.None);

        resultado.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
    }
}