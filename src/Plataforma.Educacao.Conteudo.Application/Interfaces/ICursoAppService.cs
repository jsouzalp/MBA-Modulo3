using Plataforma.Educacao.Conteudo.Application.DTO;

namespace Plataforma.Educacao.Conteudo.Application.Interfaces;
public interface ICursoAppService 
{
    Task<Guid> CadastrarCursoAsync(CadastroCursoDto dto);
    Task AtualizarCursoAsync(Guid cursoId, AtualizacaoCursoDto dto);
    Task DesativarCursoAsync(Guid cursoId);
    Task<CursoDto> ObterPorIdAsync(Guid cursoId);
    Task<IEnumerable<CursoDto>> ObterTodosAsync();
    Task<IEnumerable<CursoDto>> ObterAtivosAsync();
}