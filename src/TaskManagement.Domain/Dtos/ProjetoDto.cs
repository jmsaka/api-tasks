namespace TaskManagement.Domain.Dtos;

public class ProjetoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public ICollection<TarefaDto> Tarefas { get; set; } = [];
    public DateTime DataCriacao { get; set; }
}