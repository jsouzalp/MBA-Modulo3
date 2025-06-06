﻿using MediatR;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

namespace Plataforma.Educacao.Aluno.Application.Commands.AtualizarPagamento;
public class AtualizarPagamentoMatriculaCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<AtualizarPagamentoMatriculaCommand, bool>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<bool> Handle(AtualizarPagamentoMatriculaCommand request, CancellationToken cancellationToken)
    {
        // TODO :: Devo manter este Command? 
        // Quem faz a orquestração de dizer que MatriculaCurso foi pago é o BC de Faturamento!
        // Revisar e aguardar a opinião do Eduardo

        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return false; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return false; }

        var matricula = aluno.ObterMatriculaPorCursoId(request.CursoId);
        aluno.AtualizarPagamentoMatricula(matricula.Id);

        await _alunoRepository.AtualizarAsync(aluno);
        await _alunoRepository.UnitOfWork.Commit();

        return true;
    }

    private bool ValidarRequisicao(AtualizarPagamentoMatriculaCommand request)
    {
        request.DefinirValidacao(new AtualizarPagamentoMatriculaCommandValidator().Validate(request));
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

}
