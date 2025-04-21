using MediatR;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;

namespace Plataforma.Educacao.Aluno.Application.Commands.ConcluirCurso;
public class ConcluirCursoCommandHandler(IAlunoRepository alunoRepository,
    ICursoAppService cursoService,
    IMediatorHandler mediatorHandler) : IRequestHandler<ConcluirCursoCommand, bool>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private readonly ICursoAppService _cursoService = cursoService;
    private Guid _raizAgregacao;

    public async Task<bool> Handle(ConcluirCursoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return false; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return false; }
        var matriculaCurso = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId);

        if (!ObterCurso(matriculaCurso.CursoId, out CursoDto cursoDto)) { return false; }
        if (!ValidarSeMatriculaCursoPodeSerConcluido(aluno, cursoDto)) { return false; }

        aluno.ConcluirCurso(request.MatriculaCursoId);

        await _alunoRepository.AtualizarAsync(aluno);
        return await _alunoRepository.UnitOfWork.Commit();
    }

    private bool ValidarRequisicao(ConcluirCursoCommand request)
    {
        request.DefinirValidacao(new ConcluirCursoCommandValidator().Validate(request));
        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        return true;
    }

    private bool ObterAluno(Guid alunoId, out Domain.Entities.Aluno aluno)
    {
        aluno = _alunoRepository.ObterPorIdAsync(alunoId).Result;
        if (aluno == null)
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aluno não encontrado.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }

    private bool ObterCurso(Guid cursoId, out CursoDto cursoDto)
    {
        cursoDto = _cursoService.ObterPorIdAsync(cursoId).Result;
        if (cursoDto == null || !cursoDto.CursoDisponivel)
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Curso indisponível para matrícula.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }

    private bool ValidarSeMatriculaCursoPodeSerConcluido(Aluno.Domain.Entities.Aluno aluno, CursoDto cursoDto)
    {
        bool retorno = true;
        if (aluno.ObterQuantidadeAulasPendenteMatriculaCurso(cursoDto.Id) > 0)
        {
            retorno = false;
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Existem aulas pendentes para este curso")).GetAwaiter().GetResult();
        }

        int totalCursosAtivos = cursoDto.Aulas.Count(x => x.Ativo);
        int totalAulasMatricula = aluno.ObterQuantidadeAulasMatriculaCurso(cursoDto.Id);
        if (totalAulasMatricula < totalCursosAtivos)
        {
            retorno = false;
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Curso não pode ser concluído. Aulas pendentes.")).GetAwaiter().GetResult();
        }

        return retorno;
    }
}
