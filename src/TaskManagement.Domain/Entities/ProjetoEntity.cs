using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Entities;

public class ProjetoEntity : IEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public ICollection<TarefaEntity> Tarefas { get; set; } = [];
    public DateTime DataCriacao { get; set; }
}
