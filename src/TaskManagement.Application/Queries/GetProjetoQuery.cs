namespace TaskManagement.Application.Queries;

public class GetProjetoQuery : IRequest<BaseResponse<ICollection<ProjetoDto>>>
{
    public Guid Id { get; set; }
}
