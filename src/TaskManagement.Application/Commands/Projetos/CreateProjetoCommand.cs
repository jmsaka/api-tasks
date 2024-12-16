namespace TaskManagement.Application.Commands.Projetos;

public class CreateProjetoCommand : IRequest<Guid>
{
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
}
