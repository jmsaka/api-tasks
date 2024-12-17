namespace TaskManagement.Application.Commands.Projetos;

public class DeleteProjetoCommand : IRequest<BaseResponse<ProjetoDto>>
{
    public Guid Id { get; set; }
}
