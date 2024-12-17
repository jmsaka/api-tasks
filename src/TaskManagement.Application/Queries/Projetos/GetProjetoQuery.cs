namespace TaskManagement.Application.Queries.Projetos;

public class GetProjetoQuery : IRequest<BaseResponse<ICollection<ProjetoDto>>>
{
    public Guid Id { get; set; }
}
