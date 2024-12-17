using TaskManagement.Domain.Contracts;

namespace TaskManagement.Application.Commands.Projetos;

public class UpsertProjetoCommand : IRequest<BaseResponse<Guid>>
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
}
