namespace TaskManagement.Application.Profiles;

public class HistoricoAtualizacaoProfile : Profile
{
    public HistoricoAtualizacaoProfile()
    {
        CreateMap<HistoricoAtualizacaoEntity, HistoricoAtualizacaoDto>().ReverseMap();
    }
}