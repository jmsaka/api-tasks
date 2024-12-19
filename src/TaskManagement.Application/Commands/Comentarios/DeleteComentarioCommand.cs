namespace TaskManagement.Application.Commands.Comentarios;

public class DeleteComentarioCommand : IRequest<BaseResponse<ComentarioDto>>
{
    public Guid Id { get; set; }
}