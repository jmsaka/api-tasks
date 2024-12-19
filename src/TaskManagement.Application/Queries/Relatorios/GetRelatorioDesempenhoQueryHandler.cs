namespace TaskManagement.Application.Queries.Relatorios;

public class GetRelatorioDesempenhoQueryHandler(IRepository<TarefaEntity> repository, IMapper mapper) : IRequestHandler<GetRelatorioDesempenhoQuery, BaseResponse<ICollection<RelatorioDesempenhoDto>>>
{
    private readonly IRepository<TarefaEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BaseResponse<ICollection<RelatorioDesempenhoDto>>> Handle(GetRelatorioDesempenhoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var relatorio = _mapper.Map<ICollection<RelatorioDesempenhoDto>>(await _repository.ObterRelatorioDesempenhoAsync(request.UltimosDias, cancellationToken));

            if (relatorio == null)
            {
                return new BaseResponse<ICollection<RelatorioDesempenhoDto>>(null, false, $"Relatório não possui dados.");
            }

            return new BaseResponse<ICollection<RelatorioDesempenhoDto>>(relatorio);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<RelatorioDesempenhoDto>>(null, false, ex.Message);
        }
    }
}
