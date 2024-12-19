namespace TaskManagement.Application.Profiles;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<UpsertTarefaCommand, TarefaEntity>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap<TarefaEntity, TarefaDto>();
    }
}