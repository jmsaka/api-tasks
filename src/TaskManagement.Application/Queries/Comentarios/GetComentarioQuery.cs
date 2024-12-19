namespace TaskManagement.Application.Queries.Comentarios;

public class GetComentarioQuery : IRequest<BaseResponse<ICollection<ComentarioDto>>>
{
    public Guid Id { get; set; }
}
