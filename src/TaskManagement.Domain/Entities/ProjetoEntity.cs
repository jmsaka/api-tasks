namespace TaskManagement.Domain.Entities;

public class ProjetoEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public ICollection<TarefaEntity> Tarefas { get; set; } = [];
    public DateTime DataCriacao { get; set; }
}
