using AutoMapper;
using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Api.ViewModels.Aluno.Queries;
using Plataforma.Educacao.Api.ViewModels.ConteudoProgramatico;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Core.SharedDto.Aluno;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

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

        CreateMap<EvolucaoAlunoDto, EvolucaoAlunoViewModel>();
        CreateMap<EvolucaoMatriculaCursoDto, EvolucaoMatriculaCursoViewModel>();

        CreateMap<AulaCursoDto, AulaCursoViewModel>();        
    }
}