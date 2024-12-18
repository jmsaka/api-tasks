namespace TaskManagement.Application.Queries.HistoricoAtualizacoes;

public class GetHistoricoAtualizacaoQueryHandler(IRepository<HistoricoAtualizacaoEntity> repository,
                                                 IMapper mapper) : IRequestHandler<GetHistoricoAtualizacaoQuery, BaseResponse<ICollection<HistoricoAtualizacaoDto>>>
{
    private readonly IRepository<HistoricoAtualizacaoEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BaseResponse<ICollection<HistoricoAtualizacaoDto>>> Handle(GetHistoricoAtualizacaoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var relatorio = _mapper.Map<ICollection<HistoricoAtualizacaoDto>>(await _repository.GetAllAsync(cancellationToken));

            if (relatorio == null)
            {
                return new BaseResponse<ICollection<HistoricoAtualizacaoDto>>(null, false, $"Histórico de Atualizações não possui dados.");
            }

            return new BaseResponse<ICollection<HistoricoAtualizacaoDto>>(relatorio);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<HistoricoAtualizacaoDto>>(null, false, ex.Message);
        }
    }
}