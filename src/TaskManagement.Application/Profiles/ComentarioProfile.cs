namespace TaskManagement.Application.Profiles;

public class ComentarioProfile : Profile
{
    public ComentarioProfile()
    {
        CreateMap<UpsertComentarioCommand, ComentarioEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Comentario, opt => opt.MapFrom(src => src.Comentario))
            .ReverseMap();

        CreateMap<ComentarioEntity, ComentarioDto>()
            .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao))
            .ReverseMap();
    }
}