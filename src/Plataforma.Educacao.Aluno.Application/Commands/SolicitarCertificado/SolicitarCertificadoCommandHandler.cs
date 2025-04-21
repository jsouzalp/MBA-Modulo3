using MediatR;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Core.Messages.Comunications.AlunoCommands;

namespace Plataforma.Educacao.Aluno.Application.Commands.SolicitarCertificado;
public class SolicitarCertificadoCommandHandler(IAlunoRepository alunoRepository, IMediatorHandler mediatorHandler) : IRequestHandler<SolicitarCertificadoCommand, bool>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<bool> Handle(SolicitarCertificadoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return false; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return false; }

        aluno.RequisitarCertificadoConclusao(request.MatriculaCursoId, request.PathCertificado);
        var certificado = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId).Certificado;

        await _alunoRepository.AdicionarCertificadoMatriculaCursoAsync(certificado);
        return await _alunoRepository.UnitOfWork.Commit();
    }

    private bool ValidarRequisicao(SolicitarCertificadoCommand request)
    {
        request.DefinirValidacao(new SolicitarCertificadoCommandValidator().Validate(request));

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
