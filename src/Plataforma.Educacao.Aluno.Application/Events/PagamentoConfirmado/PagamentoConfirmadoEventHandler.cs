using MediatR;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plataforma.Educacao.Core.Messages.Integration;
using Azure.Core;
using Plataforma.Educacao.Aluno.Application.Events.AtualizarPagamento;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Conteudo.Application.Interfaces;

namespace Plataforma.Educacao.Aluno.Application.Events.PagamentoConfirmado;
public class PagamentoConfirmadoEventHandler(IAlunoRepository alunoRepository, 
    ICursoAppService cursoService,
    IMediatorHandler mediatorHandler) : INotificationHandler<PagamentoConfirmadoEvent>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly ICursoAppService _cursoService = cursoService;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task Handle(PagamentoConfirmadoEvent notification, CancellationToken cancellationToken)
    {
        _raizAgregacao = notification.RaizAgregacao;
        if (!ValidarRequisicao(notification)) { return; }
        if (!ObterCurso(notification.CursoId, out CursoDto cursoDto)) { return; }
        if (!ObterAluno(notification.AlunoId, out Domain.Entities.Aluno aluno)) { return; }

        var matricula = aluno.ObterMatriculaPorCursoId(notification.CursoId);
        aluno.AtualizarPagamentoMatricula(matricula.Id);

        await _alunoRepository.AtualizarAsync(aluno);
        await _alunoRepository.UnitOfWork.Commit();
    }

    private bool ValidarRequisicao(PagamentoConfirmadoEvent notification)
    {
        notification.DefinirValidacao(new PagamentoConfirmadoEventValidator().Validate(notification));
        if (!notification.EhValido())
        {
            foreach (var erro in notification.Erros)
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
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Curso indisponível para confirmação de pagamento.")).GetAwaiter().GetResult();
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