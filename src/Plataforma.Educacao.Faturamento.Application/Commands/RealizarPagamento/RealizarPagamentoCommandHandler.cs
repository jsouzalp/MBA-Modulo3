using MediatR;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;
using Plataforma.Educacao.Faturamento.Domain.ValueObjects;
using Plataforma.Educacao.Aluno.Application.Interfaces;
using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoEvents;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoCommands;

namespace Plataforma.Educacao.Faturamento.Application.Commands.RealizarPagamento;
public class RealizarPagamentoCommandHandler(IFaturamentoRepository faturamentoRepository,
    IAlunoQueryService alunoQueryService,
    IMediatorHandler mediatorHandler) : IRequestHandler<RealizarPagamentoCommand, bool>
{
    private readonly IFaturamentoRepository _faturamentoRepository = faturamentoRepository;
    private readonly IAlunoQueryService _alunoQueryService = alunoQueryService;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<bool> Handle(RealizarPagamentoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicaoAsync(request)) { return false; }
        if (!ObterPagamentoMatriculaCurso(request.MatriculaCursoId, out Pagamento pagamento)) { return false; }
        if (!ObterMatriculaCurso(request.MatriculaCursoId, out MatriculaCursoDto matriculaCurso)) { return false; }
        if (!ValidarValorPagamentoMatriculaCurso(request.Valor, pagamento?.Valor ?? matriculaCurso.Valor)) { return false; }

        if (!matriculaCurso.PagamentoPodeSerRealizado)
        {
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), "Matricula não permite pagamento. Entre em contato com nosso SAC"));
            return false;
        }

        bool ehInclusaoPagamento = pagamento == null;
        var dadosCartao = new DadosCartao(request.NumeroCartao, request.NomeTitularCartao, request.ValidadeCartao, request.CvvCartao);
        pagamento ??= new Pagamento(request.MatriculaCursoId, request.Valor, DateTime.Now.Date);








        #region Pendente de definição
        // TODO :: Refatorar os pontos abaixo:
        // 1) Este Handle precisa ser refatorado porque neste momento deve ser acionado algum proxy de pagamento para poder confirmar ou não o pagamento
        // 2) [OK-Falta testar] Deve ser lançado também alguma coisa para atualizar o estado da matrícula caso o pagamento seja aprovado
        // 3) [OK-Falta testar] Caso o pagamento não seja autorizado, deve ser criado um evento para notificar o Aluno. A ideia é enviar uma notificação de envio de email
        //    As variáveis abaixo foram criadas apenas para dar um melhor sentido no fluxo atual
        bool confirmado = true;
        string comprovante = "90874231390128301";

        if (!confirmado)
        {
            await _mediatorHandler.PublicarEvento(new PagamentoRecusadoEvent(matriculaCurso.Id, matriculaCurso.AlunoId, matriculaCurso.CursoId, "Pagamento foi recusado pela operadora"));
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), "Este pagamento não foi confirmado. Tente novamente ou entre em contato com nosso SAC"));
            return false;
        }

        pagamento.ConfirmarPagamento(DateTime.Now.Date, comprovante, dadosCartao);
        await _mediatorHandler.PublicarEvento(new PagamentoConfirmadoEvent(matriculaCurso.Id, matriculaCurso.AlunoId, matriculaCurso.CursoId));

        if (ehInclusaoPagamento) { await _faturamentoRepository.AdicionarAsync(pagamento); }
        else await _faturamentoRepository.AtualizarAsync(pagamento);
        #endregion Pendente de definição






        await _faturamentoRepository.UnitOfWork.Commit();

        return true;
    }

    private bool ValidarRequisicaoAsync(RealizarPagamentoCommand request)
    {
        request.DefinirValidacao(new RealizarPagamentoCommandValidator().Validate(request));
        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        return true;
    }

    private bool ObterPagamentoMatriculaCurso(Guid matriculaId, out Pagamento pagamento)
    {
        pagamento = _faturamentoRepository.ObterPorMatriculaIdAsync(matriculaId).Result;

        if (pagamento != null)
        {
            if (pagamento.PossuiPagamentoAprovado())
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), "Pagamento desta Matricula já se encontra paga")).GetAwaiter().GetResult();
                return false;
            }
        }

        return true;
    }

    private bool ObterMatriculaCurso(Guid matriculaCursoId, out MatriculaCursoDto matriculaCurso)
    {
        matriculaCurso = _alunoQueryService.ObterInformacaoMatriculaCursoParaPagamentoAsync(matriculaCursoId).Result;
        if (matriculaCurso == null)
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), "Matricula do aluno não encontrada")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }

    private bool ValidarValorPagamentoMatriculaCurso(decimal valorInformado, decimal valorMatricula)
    {
        if (valorInformado != valorMatricula)
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Pagamento), "Valor de pagamento diverge do valor desta matricula")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }
}