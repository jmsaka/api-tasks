using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Entities;

public class TarefaEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public string Status { get; set; } = "Pendente"; // Pendente, Em Andamento, Concluída
    public string Prioridade { get; set; } = "Média"; // Baixa, Média, Alta
    public Guid ProjetoId { get; set; }
    public required ProjetoEntity Projeto { get; set; }
}