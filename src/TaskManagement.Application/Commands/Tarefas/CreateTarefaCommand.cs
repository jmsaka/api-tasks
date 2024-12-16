using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.Commands.Tarefas;

public class CreateTarefaCommand : IRequest<Guid>
{
    public Guid ProjetoId { get; set; }
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public required string Prioridade { get; set; } // Valores: "Baixa", "Média", "Alta"
}