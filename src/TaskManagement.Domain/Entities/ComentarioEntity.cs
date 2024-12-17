namespace TaskManagement.Domain.Entities;

public class ComentarioEntity : IEntity
{
    public Guid Id { get; set; }
    public string? Comentario { get; set; }

    public Guid TarefaId { get; set; } 
    public TarefaEntity? Tarefa { get; set; } 
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;  
}
