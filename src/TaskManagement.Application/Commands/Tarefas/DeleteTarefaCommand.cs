namespace TaskManagement.Application.Commands.Tarefas;

public class DeleteTarefaCommand : IRequest<BaseResponse<TarefaDto>>
{
    public Guid Id { get; set; }
}
