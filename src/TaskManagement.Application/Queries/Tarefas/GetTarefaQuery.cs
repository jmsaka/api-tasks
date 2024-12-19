namespace TaskManagement.Application.Queries.Tarefas;

public class GetTarefaQuery : IRequest<BaseResponse<ICollection<TarefaDto>>>
{
    public Guid Id { get; set; }
}
