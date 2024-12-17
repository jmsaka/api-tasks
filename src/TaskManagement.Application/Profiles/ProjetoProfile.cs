using TaskManagement.Application.Commands.Projetos;

namespace TaskManagement.Application.Profiles;

public class ProjetoProfile : Profile
{
    public ProjetoProfile()
    {
        CreateMap<UpsertProjetoCommand, ProjetoEntity>().ReverseMap();
        CreateMap<ProjetoEntity, ProjetoDto>().ReverseMap();
    }
}
