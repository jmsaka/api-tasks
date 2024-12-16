namespace TaskManagement.Application.Profiles;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<CreateTarefaCommand, TarefaEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            //.ForMember(dest => dest.Historico, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Pendente")); // Status inicial
    }
}