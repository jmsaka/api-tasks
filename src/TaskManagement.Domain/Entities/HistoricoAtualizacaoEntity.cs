namespace TaskManagement.Domain.Entities;

public class HistoricoAtualizacaoEntity : IEntity
{
    public Guid Id { get; set; }
    public string? Operacao { get; set; }
    public string? Entidade { get; set; }
    public string? ValorAntigo { get; set; }
    public string? ValorNovo { get; set; }
    public DateTime DataAlteracao { get; set; }
    public string? UsuarioResponsavel { get; set; }
}