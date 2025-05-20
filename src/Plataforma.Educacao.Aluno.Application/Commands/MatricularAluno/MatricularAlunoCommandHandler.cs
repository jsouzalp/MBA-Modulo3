﻿using MediatR;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

namespace Plataforma.Educacao.Aluno.Application.Commands.MatricularAluno;
public class MatricularAlunoCommandHandler(IAlunoRepository alunoRepository,
    ICursoAppService cursoService,
    IMediatorHandler mediatorHandler) : IRequestHandler<MatricularAlunoCommand, bool>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly ICursoAppService _cursoService = cursoService;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<bool> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return false; }
        if (!ObterCurso(request.CursoId, out CursoDto cursoDto)) { return false; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return false; }

        aluno.MatricularEmCurso(request.CursoId, cursoDto.Nome, cursoDto.Valor);
        var matricula = aluno.ObterMatriculaPorCursoId(request.CursoId);
        await _alunoRepository.AdicionarMatriculaCursoAsync(matricula);
        await _alunoRepository.UnitOfWork.Commit();

        await _mediatorHandler.PublicarEvento(new GerarLinkPagamentoEvent(matricula.Id, request.AlunoId, request.CursoId, cursoDto.Valor));

        return true;
    }

    private bool ValidarRequisicao(MatricularAlunoCommand request)
    {
        request.DefinirValidacao(new MatricularAlunoCommandValidator().Validate(request));
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
}
