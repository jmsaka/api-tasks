namespace TaskManagement.Application.Commands.Tarefas;

public class UpsertTarefaCommand : IRequest<BaseResponse<Guid>>
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public required string Prioridade { get; set; } // Valores: "Baixa", "Média", "Alta"
}