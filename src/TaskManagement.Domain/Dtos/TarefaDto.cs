namespace TaskManagement.Domain.Dtos;

public class TarefaDto
{
    public Guid Id { get; set; }
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public required string Status { get; set; }
    public required string Prioridade { get; set; }
}