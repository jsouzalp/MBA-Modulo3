using AutoMapper;
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


        //CreateMap<Category, CategoryUpdateViewModel>().ReverseMap();

        //CreateMap<BudgetViewModel, Budget>();
        //CreateMap<Budget, BudgetUpdateViewModel>().ReverseMap();
        //CreateMap<Budget, BudgetViewModel>()
        //    .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Category.Description));

        //CreateMap<GeneralBudget, GeneralBudgetViewModel>().ReverseMap();

        //CreateMap<Transaction, TransactionViewModel>().ReverseMap();
        //CreateMap<Transaction, TransactionUpdateViewModel>().ReverseMap();
        //CreateMap<BalanceDTO, BalanceViewModel>().ReverseMap();
        //CreateMap<BalanceDTO, Transaction>().ReverseMap();
        //CreateMap<Transaction, BalanceDTO>()
        //    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Description))
        //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Category.Type));

    }
}