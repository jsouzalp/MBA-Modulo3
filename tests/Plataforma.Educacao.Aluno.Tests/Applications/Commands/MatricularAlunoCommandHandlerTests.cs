using FluentAssertions;
using Moq;
using Plataforma.Educacao.Aluno.Application.Commands.MatricularAluno;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

namespace Plataforma.Educacao.Aluno.Tests.Applications.Commands;
public class MatricularAlunoCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Mock<ICursoAppService> _cursoServiceMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;

    private readonly MatricularAlunoCommandHandler _handler;

    public MatricularAlunoCommandHandlerTests()
    {
        _alunoRepositoryMock = new Mock<IAlunoRepository>();
        _cursoServiceMock = new Mock<ICursoAppService>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        _handler = new MatricularAlunoCommandHandler(
            _alunoRepositoryMock.Object,
            _cursoServiceMock.Object,
            _mediatorHandlerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Deve_Matricular_Aluno_Quando_Tudo_Valido()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var aluno = new Domain.Entities.Aluno("Jairo Azevedo", "jairo.azevedo@email.com", new DateTime(1973, 06, 25));

        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync(aluno);
        _cursoServiceMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync(new CursoDto { Id = cursoId, Nome = "Curso de DDD do básico ao avançado", Valor = 1000, CursoDisponivel = true });

        var command = new MatricularAlunoCommand(alunoId, cursoId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        //_alunoRepositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Domain.Entities.Aluno>()), Times.Once);
        //_alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Publicar_Notificacao_Se_Curso_Indisponivel()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        _cursoServiceMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync(new CursoDto { Id = cursoId, CursoDisponivel = false });

        var command = new MatricularAlunoCommand(alunoId, cursoId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Publicar_Notificacao_Se_Aluno_Nao_Encontrado()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        _cursoServiceMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync(new CursoDto { Id = cursoId, CursoDisponivel = true });
        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync((Domain.Entities.Aluno)null);

        var command = new MatricularAlunoCommand(alunoId, cursoId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Publicar_Notificacao_Se_Command_Invalido()
    {
        var command = new MatricularAlunoCommand(Guid.Empty, Guid.Empty);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Handle_Deve_retornar_false_se_commit_falhar()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var aluno = new Domain.Entities.Aluno("Jairo", "jairo.souza@email.com", new DateTime(1973, 06, 25));

        _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync(aluno);
        _cursoServiceMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync(new CursoDto { Id = cursoId, Nome = "Curso de SOLID", Valor = 800, CursoDisponivel = false });

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(false);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        var command = new MatricularAlunoCommand(alunoId, cursoId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
    }
}
