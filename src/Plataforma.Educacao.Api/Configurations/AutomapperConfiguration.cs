using AutoMapper;
using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Api.ViewModels.Aluno.Queries;
using Plataforma.Educacao.Api.ViewModels.ConteudoProgramatico;
using Plataforma.Educacao.Conteudo.Application.DTO;

namespace Plataforma.Educacao.Api.Configurations;
public class AutomapperConfiguration : Profile
{
    public AutomapperConfiguration()
    {
        CreateMap<AulaViewModel, AulaDto>();
        CreateMap<CadastroCursoViewModel, CadastroCursoDto>();
        CreateMap<AtualizacaoCursoViewModel, AtualizacaoCursoDto>();

        CreateMap<AlunoDto, AlunoViewModel>();
        CreateMap<MatriculaCursoDto, MatriculaCursoViewModel>();
        CreateMap<CertificadoDto, CertificadoViewModel>();

    }
}