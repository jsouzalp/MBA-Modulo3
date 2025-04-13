using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Conteudo.Domain.Interfaces;
using Plataforma.Educacao.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Conteudo.Application.Services;
public class AulaAppService(ICursoRepository cursoRepository) : BaseService, IAulaAppService
{
    private readonly ICursoRepository _cursoRepository = cursoRepository;

    public async Task<Guid> AdicionarAulaAsync(Guid cursoId, AulaDto dto)
    {
        var curso = await ObterCursoComAulaAsync(cursoId);

        curso.AdicionarAula(dto.Descricao, dto.CargaHoraria, dto.OrdemAula);
        await _cursoRepository.AtualizarAsync(curso);
        await _cursoRepository.UnitOfWork.Commit();

        var aulaAdicionada = curso.Aulas.Last();
        return aulaAdicionada.Id;
    }

    public async Task AtualizarAulaAsync(Guid cursoId, AulaDto dto)
    {
        var curso = await ObterCursoComAulaAsync(cursoId);
        var aula = curso.Aulas.FirstOrDefault(a => a.Id == dto.Id) ?? throw new DomainException("Aula não encontrada");
        if (curso.Aulas.Any(a => a.Id != dto.Id && a.OrdemAula == dto.OrdemAula)) throw new DomainException("Já existe uma aula com essa ordem.");

        aula.AlterarDescricao(dto.Descricao);
        aula.AlterarCargaHoraria(dto.CargaHoraria);
        aula.AlterarOrdemAula(dto.OrdemAula);
        if (dto.Ativo) { aula.AtivarAula(); }
        else { aula.DesativarAula(); }

        await _cursoRepository.AtualizarAsync(curso);
        await _cursoRepository.UnitOfWork.Commit();
    }

    public async Task RemoverAulaAsync(Guid cursoId, Guid aulaId)
    {
        var curso = await ObterCursoComAulaAsync(cursoId);
        var aula = curso.Aulas.FirstOrDefault(a => a.Id == aulaId) ?? throw new DomainException("Aula não encontrada");

        curso.RemoverAula(aula);

        await _cursoRepository.AtualizarAsync(curso);
        await _cursoRepository.UnitOfWork.Commit();
    }

    #region Helpers
    private async Task<Curso> ObterCursoComAulaAsync(Guid cursoId) => await _cursoRepository.ObterPorIdAsync(cursoId) ?? throw new DomainException("Curso não encontrado");
    #endregion
}
