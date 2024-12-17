namespace TaskManagement.Domain.Entities;

public class TarefaEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;
    public Guid ProjetoId { get; set; }
    public required ProjetoEntity Projeto { get; set; }
    public ICollection<ComentarioEntity> Comentarios { get; set; } = [];
}