namespace TaskManagement.Domain.Dtos;

public class RelatorioDesempenhoDto
{
    public Guid ProjetoId { get; set; }
    public string? NomeProjeto { get; set; }
    public int TarefasConcluidas { get; set; }
    public double MediaTarefasPorDia { get; set; }
}
