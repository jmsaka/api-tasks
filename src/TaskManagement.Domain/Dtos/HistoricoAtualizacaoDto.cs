namespace TaskManagement.Domain.Dtos;

public class HistoricoAtualizacaoDto
{
    public Guid Id { get; set; }
    public string? Entidade { get; set; }
    public string? ValorAntigo { get; set; }
    public string? ValorNovo { get; set; }
    public DateTime DataAlteracao { get; set; }
    public string? UsuarioResponsavel { get; set; }
}
