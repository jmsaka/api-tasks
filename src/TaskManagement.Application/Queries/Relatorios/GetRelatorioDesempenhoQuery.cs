namespace TaskManagement.Application.Queries.Relatorios;

public class GetRelatorioDesempenhoQuery : IRequest<BaseResponse<ICollection<RelatorioDesempenhoDto>>>
{
    public int UltimosDias { get; set; } = -30;
}
