namespace TaskManagement.Application.Profiles;

public class ProjetoProfile : Profile
{
    public ProjetoProfile()
    {
        CreateMap<UpsertProjetoCommand, ProjetoEntity>().ReverseMap();
        CreateMap<ProjetoEntity, ProjetoDto>()
            .ForMember(dest => dest.Tarefas, opt => opt.MapFrom(src => src.Tarefas));
    }
}
