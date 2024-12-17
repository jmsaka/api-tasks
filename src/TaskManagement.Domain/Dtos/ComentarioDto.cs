namespace TaskManagement.Domain.Dtos;

public class ComentarioDto 
{
    public Guid Id { get; set; }
    public string? Comentario { get; set; }
    public Guid TarefaId { get; set; }
    public DateTime? DataCriacao { get; set; }
}
