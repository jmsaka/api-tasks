namespace TaskManagement.Application.Commands.Comentarios;

public class UpsertComentarioCommand : IRequest<BaseResponse<Guid>>
{
    public Guid Id { get; set; }
    public Guid TarefaId { get; set; }
    public string? Comentario { get; set; }
}
