namespace TaskManagement.Domain.Dtos;

public class TarefaDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public string Status { get; set; } 
    public string Prioridade { get; set; }
}